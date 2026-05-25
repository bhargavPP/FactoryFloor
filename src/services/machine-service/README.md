# FactoryFloor.MachineService

Purpose
- Manages machine entities (CRUD) and publishes MachineCreatedEvent to RabbitMQ when a machine is created.

Endpoints (all under /api/machines and require JWT)
- GET /api/machines — list machines for tenant
- GET /api/machines/{id} — get machine
- POST /api/machines — create machine
- PUT /api/machines/{id} — update machine
- DELETE /api/machines/{id} — delete machine
- POST /api/machines/{id}/maintenance — add maintenance log
- GET /api/machines/{id}/maintenance — list maintenance logs
- GET /api/health — health check

Config
- ConnectionStrings:DefaultConnection -> Postgres for machines
- RabbitMQ:Host, RabbitMQ:Username, RabbitMQ:Password
- Jwt:Secret/Issuer/Audience

Run
- dotnet run --project src/services/machine-service/FactoryFloor.MachineService

Notes
- Publishes MachineCreatedEvent (see Contracts) using MassTransit.
- Includes Dockerfile and K8s manifests under K8s/.
