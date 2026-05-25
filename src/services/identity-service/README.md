# FactoryFloor.IdentityService

Purpose
- Provides tenant and user management and issues JWT tokens for authentication.

Endpoints
- POST /api/auth/register — register a tenant and admin user
- POST /api/auth/login — login and retrieve JWT
- GET /api/auth/health — health check

Config
- ConnectionStrings:DefaultConnection -> Postgres connection
- Jwt:Secret, Jwt:Issuer, Jwt:Audience

Run
- dotnet run --project src/services/identity-service/FactoryFloor.IdentityService

Notes
- EF Core migrations are present under Migrations/. The service attempts to auto-migrate on startup.
- TokenService generates JWTs embedding tenant_id and role claims.
