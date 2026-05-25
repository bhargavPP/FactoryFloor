# FactoryFloor — Solution Documentation

This repository contains the FactoryFloor microservices sample platform. The solution is organized as multiple services plus shared contracts and building blocks.

Quick summary of components:
- Gateway: YARP reverse proxy that routes requests to backend services and enforces JWT authentication.
- Identity Service: Postgres-based service providing tenant and user authentication and JWT issuance.
- Machine Service: Postgres-backed service that manages machines and publishes MachineCreated events to RabbitMQ.
- Telemetry Service: MongoDB-backed service that ingests telemetry readings, evaluates thresholds and publishes alerts via RabbitMQ.
- Notification Service: Consumer that receives domain events (e.g. MachineCreated, TelemetryAlert) and logs/handles notifications.
- Shared/Contracts: Event DTOs shared between services.

Quick start (local development):
1. Ensure dependencies are running: Postgres, RabbitMQ, MongoDB. You can run them via Docker compose or locally.
2. From solution root run services individually (example):
   - dotnet run --project src/services/identity-service/FactoryFloor.IdentityService
   - dotnet run --project src/services/machine-service/FactoryFloor.MachineService
   - dotnet run --project src/services/telemetry-service/FactoryFloor.TelemetryService
   - dotnet run --project src/services/notification-service/FactoryFloor.NotificationService
   - dotnet run --project src/gateway/FactoryFloor.Gateway
3. Use the Identity service to register/login and obtain a JWT. Use the JWT to call protected endpoints on other services.

Where to find more:
- docs/architecture.md — high-level architecture and component responsibilities
- docs/deployment.md — Docker and Kubernetes deployment notes
- docs/development.md — local setup, migration, and run instructions
- docs/contributing.md — contribution guidelines
- Per-project READMEs under each project folder with service-specific details
