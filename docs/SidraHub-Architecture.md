# SidraHub Business Platform

## Architecture

The solution now follows a clean architecture baseline:

- `SidraHub.Domain`: core entities and enums.
- `SidraHub.Application`: application abstractions.
- `SidraHub.Infrastructure`: EF Core, SQL Server, ASP.NET Identity, JWT wiring.
- `SidraHub.Api`: presentation layer and composition root.

## Database Design

The database is configured with Entity Framework Core Code First and includes:

- ASP.NET Identity tables for authentication and roles.
- Core business tables for services, orders, invoices, payments, consultations, providers, reviews, articles, and notifications.
- Relationship mapping for the many-to-many tables `OrderServices` and `ServiceProviders`.

## Migration Commands

Run the initial migration from the solution root:

```powershell
dotnet ef migrations add InitialCreate --project .\src\SidraHub.Infrastructure\SidraHub.Infrastructure.csproj --startup-project .\src\SidraHub.Api\SidraHub.Api.csproj --output-dir Persistence\Migrations
dotnet ef database update --project .\src\SidraHub.Infrastructure\SidraHub.Infrastructure.csproj --startup-project .\src\SidraHub.Api\SidraHub.Api.csproj
```