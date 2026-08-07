# XYZ-Shop

ASP.NET Core 8 MVC game store inspired by Steam: browse a catalog, manage games, leave reviews, chat in real time, and get recommendations from the [RAWG](https://rawg.io/apidocs) API. Optional React catalog UI lives in `react-front` (local only — not part of Docker).

## Stack

- **.NET 8** — MVC + Razor Views, SignalR
- **EF Core 8** + **SQL Server**
- **JWT** cookie authentication, BCrypt password hashing
- **Bootstrap / jQuery** front-end assets
- **React + Vite + TypeScript** — optional SPA in `react-front`
- Localization: English / Russian

## Solution structure

```text
XYZ-Shop/
├── docker-compose.yml          # App + SQL Server containers (ASP.NET + MSSQL only)
├── .env.example                # Secrets template for Docker / GCP
├── scripts/seed-data.sql       # Sample data (after migrations)
├── docs/deploy-gcp.md          # Google Cloud VM deployment
├── react-front/                # React catalog SPA (local `npm run dev`)
└── XYZ-shop/
    ├── XYZ-shop.Web            # MVC host, auth, hubs, views
    ├── XYZ-shop.Application    # Services, DTOs, abstractions
    ├── XYZ-shop.Domain         # Entities, enums
    ├── XYZ-shop.Infrastructure # EF Core, repositories, RAWG client
    └── XYZ-shop.Tests.E2E      # Selenium E2E tests
```

Default route: `Steam/Index` (`/{controller=Steam}/{action=Index}/{id?}`).

## Prerequisites (local)

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server or **LocalDB**
- EF Core tools: `dotnet tool install --global dotnet-ef`
- RAWG API key (for recommendations)
- Node.js 20+ (only if you run the React front-end)

## Configuration

Do **not** commit real secrets. Prefer user secrets, environment variables, or a local `.env` for Docker.

| Key | Purpose |
|-----|---------|
| `ConnectionStrings:DefaultDbConnection` | SQL Server connection |
| `Jwt:Key` | Signing key (≥ 32 characters) |
| `Jwt:Issuer` / `Jwt:Audience` / `Jwt:ExpireMinutes` | JWT settings |
| `RAWG:ApiKey` | RAWG Games API key |

Environment variable form (Docker / Linux):

```text
ConnectionStrings__DefaultDbConnection=...
Jwt__Key=...
RAWG__ApiKey=...
```

Development defaults live in `XYZ-shop/XYZ-shop.Web/appsettings.Development.json` (LocalDB). Override them for any shared or cloud environment.

## Local run

```bash
cd XYZ-shop

dotnet ef database update \
  --project XYZ-shop.Infrastructure \
  --startup-project XYZ-shop.Web

dotnet run --project XYZ-shop.Web --launch-profile https
```

- HTTP: `http://localhost:5256`
- HTTPS: `https://localhost:7063`

### React front-end (optional)

Runs locally against the ASP.NET API via Vite proxy (`/api` → `https://localhost:7063`). Not included in Docker Compose.

```bash
cd react-front
npm install
npm run dev
```

### Seed sample data

Apply schema first (`database update`), then:

```bash
sqlcmd -S "(localdb)\MSSQLLocalDB" -d XYZ-project -i scripts/seed-data.sql
```

For Docker SQL Server on port 1433:

```bash
sqlcmd -S localhost,1433 -U sa -P "Your_strong_Password1" -d XYZ-project -i scripts/seed-data.sql
```

The script is idempotent: it skips if `Games` already has rows.

### Demo accounts (after seed)

| Login  | Password   | Role       |
|--------|------------|------------|
| admin  | Admin123!  | Admin      |
| user1  | User123!   | User       |
| mod1   | Mod123!    | Moderator  |

## Docker Compose (app + MSSQL)

Two containers on one host: `web` (ASP.NET) and `db` (SQL Server 2022). React is **not** containerized.

```bash
cp .env.example .env
# Edit .env: MSSQL_SA_PASSWORD, JWT_KEY, RAWG_API_KEY

docker compose up -d --build
```

- Site: `http://localhost/`
- SQL Server: `localhost:1433`

Then create the schema and seed (same as cloud):

```bash
export ConnectionStrings__DefaultDbConnection="Server=localhost,1433;Database=XYZ-project;User Id=sa;Password=YOUR_SA_PASSWORD;TrustServerCertificate=True;Encrypt=False;"

dotnet ef database update \
  --project XYZ-shop/XYZ-shop.Infrastructure/XYZ-shop.Infrastructure.csproj \
  --startup-project XYZ-shop/XYZ-shop.Web/XYZ-shop.Web.csproj

docker cp scripts/seed-data.sql xyz-shop-db:/tmp/seed-data.sql
docker exec -it xyz-shop-db /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'YOUR_SA_PASSWORD' -C \
  -d XYZ-project -i /tmp/seed-data.sql
```

SQL Server needs roughly **2+ GB RAM** on the host.

## Deploy to Google Cloud

Target setup: **one Compute Engine VM**, **two Docker containers** (`web` + `mssql`), public HTTP on port **80**.

High-level flow:

1. Push the repo (Dockerfile, compose, seed script) to GitHub.
2. Create an Ubuntu/Debian VM (`e2-standard-2` recommended, **30 GB** disk — resize filesystem inside the OS).
3. Firewall: network tag `http-server` + TCP **80**.
4. Install Docker; `cp .env.example .env`; set SA password / JWT / RAWG; `docker compose up -d --build`.
5. Install .NET 8 + `dotnet-ef` **8.x** (fix ICU on Debian); run `database update` with `--connection` to `localhost,1433` (not LocalDB).
6. Run `scripts/seed-data.sql` via `sqlcmd` with flags **`-C -I`**.
7. Restart `web` after migrate/seed, then open `http://<VM_EXTERNAL_IP>/`.

Full walkthrough with all known pitfalls (disk resize, `!` in passwords, LocalDB, ICU, firewall, web crash before migrations):  
**[docs/deploy-gcp.md](docs/deploy-gcp.md)** (подробный гайд на русском).

## Main features

- Game catalog with filters and details
- Auth (register / login) with role-based access
- Game reviews and rating analytics background job
- Community chat and notifications (SignalR)
- Recommendations via RAWG API
- EN / RU UI language

## License

This project is provided as-is for educational / portfolio use unless otherwise specified.
