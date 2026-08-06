# Build context: repository root (d:\XYZ-Shop)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY XYZ-shop/XYZ-shop.Domain/XYZ-shop.Domain.csproj XYZ-shop/XYZ-shop.Domain/
COPY XYZ-shop/XYZ-shop.Application/XYZ-shop.Application.csproj XYZ-shop/XYZ-shop.Application/
COPY XYZ-shop/XYZ-shop.Infrastructure/XYZ-shop.Infrastructure.csproj XYZ-shop/XYZ-shop.Infrastructure/
COPY XYZ-shop/XYZ-shop.Web/XYZ-shop.Web.csproj XYZ-shop/XYZ-shop.Web/

RUN dotnet restore XYZ-shop/XYZ-shop.Web/XYZ-shop.Web.csproj

COPY XYZ-shop/XYZ-shop.Domain/ XYZ-shop/XYZ-shop.Domain/
COPY XYZ-shop/XYZ-shop.Application/ XYZ-shop/XYZ-shop.Application/
COPY XYZ-shop/XYZ-shop.Infrastructure/ XYZ-shop/XYZ-shop.Infrastructure/
COPY XYZ-shop/XYZ-shop.Web/ XYZ-shop/XYZ-shop.Web/

RUN dotnet publish XYZ-shop/XYZ-shop.Web/XYZ-shop.Web.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "XYZ-shop.Web.dll"]
