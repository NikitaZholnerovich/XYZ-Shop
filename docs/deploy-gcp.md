# Деплой XYZ-Shop в Google Cloud с нуля

Пошаговая инструкция: **одна VM** + **два Docker-контейнера** (приложение + SQL Server), создание схемы через EF Core и заполнение базы seed-скриптом.

В гайд включены типичные ошибки, которые уже встречались на практике (диск 10 GB, LocalDB, ICU, `!` в пароле, firewall, падение `web` до миграций и т.д.).

---

## Что получится в итоге

```text
Браузер  →  http://EXTERNAL_IP:80
                │
         GCP VM (Debian/Ubuntu)
                │
     ┌──────────┴──────────┐
     │  xyz-shop-web :8080 │
     │  (ASP.NET)          │──── Server=db,1433
     └─────────────────────┘           │
                              xyz-shop-db :1433
                              (SQL Server 2022)
                              БД: XYZ-project
```

| Контейнер | Роль | Порт на VM |
|-----------|------|------------|
| `xyz-shop-web` | сайт | **80** → 8080 |
| `xyz-shop-db` | MSSQL | **1433** (для миграций/seed с хоста) |

Адрес БД **внутри** Docker для приложения — всегда хост **`db`**, не `localhost` и не LocalDB.

---

## Содержание

1. [Что нужно заранее](#1-что-нужно-заранее)
2. [Запушить код в GitHub](#2-запушить-код-в-github)
3. [Создать VM](#3-создать-vm)
4. [Увеличить диск и расширить раздел](#4-увеличить-диск-и-расширить-раздел)
5. [Firewall (чтобы сайт открывался снаружи)](#5-firewall-чтобы-сайт-открывался-снаружи)
6. [SSH и проверка VM](#6-ssh-и-проверка-vm)
7. [Установить Docker](#7-установить-docker)
8. [Клонировать репозиторий и настроить `.env`](#8-клонировать-репозиторий-и-настроить-env)
9. [Запустить контейнеры](#9-запустить-контейнеры)
10. [Установить .NET SDK и dotnet-ef](#10-установить-net-sdk-и-dotnet-ef)
11. [Создать схему БД (миграции)](#11-создать-схему-бд-миграции)
12. [Заполнить БД (seed)](#12-заполнить-бд-seed)
13. [Перезапустить web и проверить сайт](#13-перезапустить-web-и-проверить-сайт)
14. [После перезагрузки VM](#14-после-перезагрузки-vm)
15. [Частые ошибки](#15-частые-ошибки)
16. [Краткая шпаргалка](#16-краткая-шпаргалка)

---

## 1. Что нужно заранее

- Аккаунт Google Cloud с **включённым биллингом**
- Репозиторий XYZ-Shop на GitHub **уже с Docker-файлами** (после пуша)
- Ключ [RAWG API](https://rawg.io/apidocs)
- JWT-ключ (≥ 32 символа) — можно взять из `appsettings.Development.json` или сгенерировать новый

### Важно про пароль SQL Server

Пароль **`MSSQL_SA_PASSWORD` вы придумываете сами**. Базы ещё нет — контейнер SQL Server создастся с этим паролем пользователя `sa`.

Требования:

- минимум **8** символов;
- заглавная + строчная + цифра + спецсимвол;
- **не используйте `$`** (ломает bash/`docker compose`);
- символ **`!` допустим**, но в командах bash всегда берите пароль в **одинарные** кавычки `'...'`.

Пример хорошего пароля: `GHgb45Pass!`

---

## 2. Запушить код в GitHub

На VM будет `git clone` / `git pull`. Без пуша на сервере не будет:

- `docker-compose.yml`
- `XYZ-shop/XYZ-shop.Web/Dockerfile`
- `.env.example`
- `scripts/seed-data.sql`

**В Git пушьте только `.env.example`.** Файл `.env` с секретами — только на VM, в git не коммитить.

---

## 3. Создать VM

Console → [Compute Engine → VM instances](https://console.cloud.google.com/compute/instances) → **Create instance**.

| Параметр | Рекомендация |
|----------|----------------|
| Name | `xyz-shop` |
| Machine type | **`e2-standard-2`** (2 vCPU, 8 GB) — минимум комфортный для MSSQL. `e2-medium` (4 GB) тоже можно, но теснее |
| Boot disk OS | **Ubuntu 22.04** или **Debian 12/13** |
| Disk size | сразу **30 GB** (если UI дал 10 GB — увеличьте после создания, см. шаг 4) |
| Firewall | галка **Allow HTTP traffic** (если есть) |

Создайте VM, дождитесь статуса **Running**, скопируйте **External IP**.

> SQL Server в Docker на 2 GB RAM часто не поднимается. `e2-standard-2` — нормальный выбор.

---

## 4. Увеличить диск и расширить раздел

Если в свойствах диска **10 GB**, а в ОС после увеличения в Console всё ещё ~10 GB — это нормально: GCP увеличил диск, а раздел Linux ещё нет.

### 4.1. В Console

1. **Stop** VM.
2. **Compute Engine → Disks** → диск этой VM → **Edit** → Size **30** (или 40) → **Save**.
3. **Start** VM.

### 4.2. В SSH (обязательно)

```bash
lsblk
df -h

sudo apt-get update
sudo apt-get install -y cloud-guest-utils
sudo growpart /dev/sda 1
sudo resize2fs /dev/sda1

df -h
```

У `/` (`/dev/sda1`) в `df -h` должно быть около **30G**, не 9.7G.

Если устройство не `sda` — смотрите `lsblk` и подставьте своё имя.

**Без этого** `docker compose build` почти наверняка упрётся в нехватку места.

---

## 5. Firewall (чтобы сайт открывался снаружи)

Симптом: на VM `curl -I http://127.0.0.1/` → **200 OK**, а в браузере по External IP страница не грузится.

### 5.1. Network tag

VM → **Edit** → **Network tags** → добавьте:

```text
http-server
```

Save.

### 5.2. Правило firewall

**VPC network → Firewall** — должно быть правило на **tcp:80** для тега `http-server` (часто `default-allow-http`).

Если нет — создайте:

| Поле | Значение |
|------|----------|
| Name | `allow-xyz-http` |
| Direction | Ingress |
| Targets | Specified target tags → `http-server` |
| Source IPv4 ranges | `0.0.0.0/0` |
| Protocols and ports | TCP **80** |

Открывайте сайт как **`http://EXTERNAL_IP/`** (не https).

> Порт **1433** наружу лучше не оставлять открытым навсегда. Для учебного стенда он проброшен, чтобы с VM делать migrate/seed. После настройки можно ограничить firewall.

---

## 6. SSH и проверка VM

VM → **SSH** → Open in browser.

```bash
free -h          # желательно ~8 GB
df -h            # / около 30G
cat /etc/os-release
```

---

## 7. Установить Docker

### Debian 13 (и похожие)

Репозиторий Docker для `trixie` может быть ещё нестабилен — используйте **bookworm**:

```bash
sudo apt-get update
sudo apt-get install -y ca-certificates curl
sudo install -m 0755 -d /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/debian/gpg -o /etc/apt/keyrings/docker.asc
sudo chmod a+r /etc/apt/keyrings/docker.asc

echo "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/debian bookworm stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt-get update
sudo apt-get install -y docker-ce docker-ce-cli containerd.io docker-compose-plugin
sudo usermod -aG docker "$USER"
newgrp docker

docker --version
docker compose version
```

### Ubuntu 22.04

Те же шаги, но URL репозитория:

`https://download.docker.com/linux/ubuntu`  
и в `echo` строке — `ubuntu` + `$(. /etc/os-release && echo "$VERSION_CODENAME")`.

Проверка: `docker run --rm hello-world` без `sudo`.

Если `permission denied` на Docker socket — перелогиньтесь в SSH или снова `newgrp docker`.

---

## 8. Клонировать репозиторий и настроить `.env`

```bash
cd ~
git clone https://github.com/ВАШ_ЮЗЕР/XYZ-Shop.git
cd XYZ-Shop
ls
```

Должны быть: `docker-compose.yml`, `.env.example`, `scripts/seed-data.sql`.

```bash
cp .env.example .env
nano .env
```

Заполните:

```text
MSSQL_SA_PASSWORD=GHgb45Pass!
JWT_KEY=ваш-ключ-минимум-32-символа
RAWG_API_KEY=ваш-rawg-ключ
```

JWT можно сгенерировать:

```bash
openssl rand -base64 48
```

**`docker-compose.yml` менять не нужно.** Там плейсхолдеры `${MSSQL_SA_PASSWORD}` и т.д. — Compose подставит значения из `.env` сам.

Адрес SQL в `.env` указывать **не надо**. Для приложения он уже в compose:

```text
Server=db,1433;Database=XYZ-project;User Id=sa;Password=...;TrustServerCertificate=True;Encrypt=False;
```

Образ MSSQL **собирать не нужно** — тянется готовый `mcr.microsoft.com/mssql/server:2022-latest`.

---

## 9. Запустить контейнеры

```bash
cd ~/XYZ-Shop
docker compose up -d --build
docker compose ps
```

Первый раз 5–15 минут (скачивание образов + сборка).

Ожидаемо:

- `xyz-shop-db` — **healthy**
- `xyz-shop-web` — **Started/Up** (может упасть до миграций — это ожидаемо, см. шаги 11–13)

Логи БД:

```bash
docker compose logs --tail=50 db
```

---

## 10. Установить .NET SDK и dotnet-ef

Нужны только чтобы выполнить `dotnet ef database update` с VM.

### 10.1. Скрипт установки SDK 8

```bash
wget https://dot.net/v1/dotnet-install.sh -O ~/dotnet-install.sh
chmod +x ~/dotnet-install.sh
~/dotnet-install.sh --channel 8.0
```

### 10.2. Ошибка ICU (часто на Debian)

Если видите:

`Couldn't find a valid ICU package` / `Aborted`

```bash
sudo apt-get update
sudo apt-get install -y libicu-dev
```

### 10.3. PATH (в каждой новой SSH-сессии)

```bash
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools"
```

Чтобы не вводить каждый раз:

```bash
echo 'export DOTNET_ROOT="$HOME/.dotnet"' >> ~/.bashrc
echo 'export PATH="$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools"' >> ~/.bashrc
```

### 10.4. dotnet-ef — версия 8, не 10

Проект на **.NET 8**. Глобальный `dotnet-ef` 10-й линии может мешать.

```bash
dotnet tool uninstall --global dotnet-ef 2>/dev/null
dotnet tool install --global dotnet-ef --version 8.0.14
dotnet ef --version
```

---

## 11. Создать схему БД (миграции)

Seed **не создаёт таблицы**. Сначала миграции.

### Типичные ошибки на этом шаге

| Ошибка | Причина | Решение |
|--------|---------|---------|
| `bash: !: event not found` | `!` в пароле в **двойных** кавычках | только **одинарные** `'...'` |
| `LocalDB is not supported on this platform` | EF взял строку из `appsettings.Development.json` | `ASPNETCORE_ENVIRONMENT=Production` + флаг `--connection` |
| `Login failed for user 'sa'` | неверный пароль / обрезан из‑за `!` | тот же пароль, что в `.env`, в `'...'` |

Команда (подставьте свой пароль):

```bash
cd ~/XYZ-Shop
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools"
export ASPNETCORE_ENVIRONMENT=Production

dotnet ef database update \
  --project XYZ-shop/XYZ-shop.Infrastructure/XYZ-shop.Infrastructure.csproj \
  --startup-project XYZ-shop/XYZ-shop.Web/XYZ-shop.Web.csproj \
  --connection 'Server=localhost,1433;Database=XYZ-project;User Id=sa;Password=GHgb45Pass!;TrustServerCertificate=True;Encrypt=False;'
```

Здесь `localhost,1433` — правильно: команда идёт **с хоста VM** в проброшенный порт контейнера.

Успех: список применённых миграций и `Done.`

Проверка таблиц:

```bash
docker exec -it xyz-shop-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'GHgb45Pass!' -C -I \
  -d XYZ-project -Q "SELECT name FROM sys.tables ORDER BY name;"
```

Должны быть `Games`, `Users`, `Publishers`, `__EFMigrationsHistory` и др.

---

## 12. Заполнить БД (seed)

```bash
cd ~/XYZ-Shop
docker cp scripts/seed-data.sql xyz-shop-db:/tmp/seed-data.sql

docker exec -it xyz-shop-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'GHgb45Pass!' -C -I \
  -d XYZ-project -i /tmp/seed-data.sql
```

Обязательно флаги:

- **`-C`** — доверять сертификату SQL в контейнере  
- **`-I`** — `QUOTED_IDENTIFIER ON` (без этого бывает ошибка на INSERT из‑за filtered index)

Ожидаемо: `Seed completed successfully...`

### Демо-логины после seed

| Login | Password | Role |
|-------|----------|------|
| `admin` | `Admin123!` | Admin |
| `user1` | `User123!` | User |
| `mod1` | `Mod123!` | Moderator |

Актуальный `scripts/seed-data.sql` заполняет популярные игры с обложками Steam CDN и может **перезаписать** предыдущие seed-данные при повторном запуске.

Если `mssql-tools18` не найден:

```bash
docker exec -it xyz-shop-db bash -c 'ls /opt/mssql-tools*/bin/sqlcmd'
```

Используйте найденный путь (иногда `/opt/mssql-tools/bin/sqlcmd`).

---

## 13. Перезапустить web и проверить сайт

### Почему web мог упасть раньше

При первом `docker compose up` контейнер `web` стартует **до** миграций. Фоновый сервис рейтингов лезет в БД `XYZ-project`, её ещё нет → процесс падает.  
После migrate + seed БД есть — нужно **поднять web снова**:

```bash
cd ~/XYZ-Shop
docker compose up -d web
docker compose ps
sleep 3
curl -I http://127.0.0.1/
```

Ожидаемо: оба контейнера **Up**, curl → **HTTP/1.1 200 OK** (или 302).

В браузере:

```text
http://EXTERNAL_IP/
```

Логин: `admin` / `Admin123!`

Если curl на VM = 200, а браузер молчит — вернитесь к [разделу 5 (firewall)](#5-firewall-чтобы-сайт-открывался-снаружи). Проверьте, что External IP не сменился после Stop/Start (если IP не static).

Логи при проблемах:

```bash
docker compose logs --tail=80 web
docker exec xyz-shop-web printenv ConnectionStrings__DefaultDbConnection
```

---

## 14. После перезагрузки VM

Данные БД хранятся в Docker volume — migrate/seed **заново не нужны**.

```bash
cd ~/XYZ-Shop
docker compose up -d
docker compose ps
curl -I http://127.0.0.1/
```

Чтобы контейнеры поднимались сами после reboot:

```bash
docker update --restart unless-stopped xyz-shop-db xyz-shop-web
```

(В актуальном `docker-compose.yml` у `web` уже может быть `restart: unless-stopped` — после `git pull` и recreate.)

---

## 15. Частые ошибки

| Симптом | Что делать |
|---------|------------|
| `df -h` показывает ~10G после увеличения диска в GCP | `growpart` + `resize2fs` |
| Нехватка места при build | диск ≥ 30 GB + resize в ОС |
| `ICU` / `Aborted` у `dotnet` | `sudo apt-get install -y libicu-dev` |
| `LocalDB is not supported` | `--connection 'Server=localhost,1433;...'` + `ASPNETCORE_ENVIRONMENT=Production` |
| `!: event not found` | одинарные кавычки вокруг пароля с `!` |
| `QUOTED_IDENTIFIER` при seed | флаг sqlcmd **`-I`** |
| `web` Up less than a second / Empty reply | сначала migrate+seed, потом `docker compose up -d web` |
| `Cannot open database XYZ-project` у web | схема ещё не создана — шаг 11, затем рестарт web |
| Сайт только с VM, не из браузера | tag `http-server` + firewall tcp/80, URL именно `http://` |
| `dotnet-ef` 10.x | поставить `8.0.14` |
| Сменили `MSSQL_SA_PASSWORD` в `.env` на уже живом томе | старый SA-пароль в volume не меняется сам; либо вернуть старый пароль в `.env`, либо `docker compose down -v` (сотрёт БД) и всё с нуля |

---

## 16. Краткая шпаргалка

```bash
# --- на VM, один раз после создания ---
# диск 30G + growpart/resize2fs
# Docker установлен
# firewall: http-server + tcp/80

cd ~
git clone https://github.com/ВАШ_ЮЗЕР/XYZ-Shop.git
cd XYZ-Shop
cp .env.example .env && nano .env          # SA / JWT / RAWG

docker compose up -d --build
# дождаться healthy у db

sudo apt-get install -y libicu-dev
~/dotnet-install.sh --channel 8.0          # если ещё не ставили
export DOTNET_ROOT="$HOME/.dotnet"
export PATH="$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools"
dotnet tool install --global dotnet-ef --version 8.0.14

export ASPNETCORE_ENVIRONMENT=Production
dotnet ef database update \
  --project XYZ-shop/XYZ-shop.Infrastructure/XYZ-shop.Infrastructure.csproj \
  --startup-project XYZ-shop/XYZ-shop.Web/XYZ-shop.Web.csproj \
  --connection 'Server=localhost,1433;Database=XYZ-project;User Id=sa;Password=ВАШ_ПАРОЛЬ!;TrustServerCertificate=True;Encrypt=False;'

docker cp scripts/seed-data.sql xyz-shop-db:/tmp/seed-data.sql
docker exec -it xyz-shop-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'ВАШ_ПАРОЛЬ!' -C -I \
  -d XYZ-project -i /tmp/seed-data.sql

docker compose up -d web
curl -I http://127.0.0.1/
# браузер: http://EXTERNAL_IP/
```

### Порядок критичен

1. Контейнеры (`db` healthy)  
2. **Миграции** (таблицы)  
3. **Seed** (данные)  
4. **Рестарт `web`**  
5. Firewall + проверка External IP  

Не открывайте сайт «сразу после compose», пока не сделаны шаги 2–4.

---

## Полезные команды

```bash
docker compose ps
docker compose logs -f web
docker compose logs -f db
docker compose restart web
docker compose down          # остановить, volume с БД сохранить
docker compose down -v       # остановить и УДАЛИТЬ данные БД
```
