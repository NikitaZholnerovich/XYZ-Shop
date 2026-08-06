# XYZ-Shop

ASP.NET Core 8 MVC game store inspired by Steam: browse a catalog, manage games, leave reviews, chat in real time, and get recommendations from the [RAWG](https://rawg.io/apidocs) API.

## Stack

- **.NET 8** — MVC + Razor Views, SignalR
- **EF Core 8** + **SQL Server**
- **JWT** cookie authentication, BCrypt password hashing
- **Bootstrap / jQuery** front-end assets
- Localization: English / Russian

## Solution structure

```text
XYZ-Shop/
├── docker-compose.yml          # App + SQL Server containers
├── .env.example                # Secrets template for Docker / GCP
├── scripts/seed-data.sql       # Sample data (after migrations)
├── docs/deploy-gcp.md          # Google Cloud VM deployment
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

Two containers on one host: `web` (ASP.NET) and `db` (SQL Server 2022).

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

1. Create an Ubuntu VM (`e2-medium` or larger — SQL Server needs RAM).
2. Open firewall TCP **80**; install Docker Compose on the VM.
3. Clone the repo, configure `.env`, run `docker compose up -d --build`.
4. Apply schema with `dotnet ef database update`, then run `scripts/seed-data.sql`.
5. Open `http://<VM_EXTERNAL_IP>/`.

Full walkthrough (project setup, firewall, SSH, `.env`, migrations, seed, verification, updates, troubleshooting, costs):  
**[docs/deploy-gcp.md](docs/deploy-gcp.md)**.

## Main features

- Game catalog with filters and details
- Auth (register / login) with role-based access
- Game reviews and rating analytics background job
- Community chat and notifications (SignalR)
- Recommendations via RAWG API
- EN / RU UI language

## License

This project is provided as-is for educational / portfolio use unless otherwise specified.
