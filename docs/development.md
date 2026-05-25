# Development Guide

Local development prerequisites
- .NET 10 SDK
- Docker & Docker Compose (optional for dependencies)
- PostgreSQL, RabbitMQ, MongoDB (can run via Docker)

Running dependencies with Docker Compose (example)
- You can create a docker-compose.yml to run Postgres, RabbitMQ, and MongoDB. Example services:
  - postgres: image: postgres:15
  - rabbitmq: image: rabbitmq:3-management
  - mongo: image: mongo:6

Running services locally
- From solution root, run individual services:
  - dotnet run --project src/services/identity-service/FactoryFloor.IdentityService
  - dotnet run --project src/services/machine-service/FactoryFloor.MachineService
  - dotnet run --project src/services/telemetry-service/FactoryFloor.TelemetryService
  - dotnet run --project src/services/notification-service/FactoryFloor.NotificationService
  - dotnet run --project src/gateway/FactoryFloor.Gateway

Database migrations
- Identity and Machine services use EF Core migrations. On startup they attempt to auto-migrate. To run migrations manually:
  - dotnet ef migrations add <Name> --project src/services/identity-service/FactoryFloor.IdentityService --startup-project src/services/identity-service/FactoryFloor.IdentityService
  - dotnet ef database update --project ...

Configuration
- Use appsettings.Development.json for local dev or environment variables. Important keys:
  - Jwt:Secret (min 32 chars), Jwt:Issuer, Jwt:Audience
  - ConnectionStrings:DefaultConnection (Postgres)
  - MongoDB:ConnectionString, MongoDB:DatabaseName
  - RabbitMQ:Host, RabbitMQ:Username, RabbitMQ:Password

Testing flows
- Use the Identity service to register a tenant and login to get a token.
- Use the token to call Machine Service endpoints and Telemetry ingestion endpoints.
- Telemetry service README contains an example PowerShell script to ingest readings and observe alert behavior.

Developer tips
- Use Swagger UIs exposed by services for quick API tests.
- Use MassTransit test harness or inspect RabbitMQ management UI for messages.
- Keep JWT secret out of source control; use environment variables or secret stores.
