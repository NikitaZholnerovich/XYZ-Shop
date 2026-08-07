# XYZ Shop — React front-end

Optional React + TypeScript + Vite SPA for the XYZ Shop game catalog.

Proxies `/api` to the ASP.NET backend at `https://localhost:7063` (see `vite.config.ts`).

## Run locally

1. Start the backend: `dotnet run --project XYZ-shop/XYZ-shop.Web --launch-profile https`
2. In this folder:

```bash
npm install
npm run dev
```

Not deployed with Docker Compose — use ASP.NET MVC for production/cloud.
