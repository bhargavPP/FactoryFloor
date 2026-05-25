# Architecture Overview

FactoryFloor is a modular microservices system designed to manage machines, ingest telemetry, and notify stakeholders on important events. Key architectural decisions:

- Microservices: Each bounded context (Identity, Machine, Telemetry, Notification) is implemented as a separate ASP.NET Minimal API service.
- API Gateway: FactoryFloor.Gateway uses YARP (ReverseProxy) to route requests to downstream services and apply authentication policies.
- Messaging: RabbitMQ (via MassTransit) is used for asynchronous communication between services for domain events (e.g., MachineCreated, TelemetryAlert).
- Data Stores:
  - Identity & Machine services: PostgreSQL (EF Core) for relational data.
  - Telemetry service: MongoDB for time-series-like telemetry readings.
- Authentication: JWT-based authentication issued by Identity service. Services validate tokens using configured issuer/audience/secret.
- Observability: Services expose Swagger for API exploration and basic health endpoints.

Component responsibilities:
- Gateway: Accepts incoming HTTP requests, validates JWTs, and forwards to appropriate service clusters.
- Identity Service: Tenant and user management, token issuance, migrations for user/tenant schema.
- Machine Service: CRUD for machine entities, publishes MachineCreatedEvent on creation.
- Telemetry Service: Ingests telemetry readings, stores them in MongoDB, evaluates thresholds, publishes TelemetryAlertEvent.
- Notification Service: Consumes events and performs notification actions (logging, external systems integration).

Interactions:
- User -> Gateway -> Identity (login/register)
- User -> Gateway -> Machine (manage machines)
- MachineService publishes MachineCreatedEvent -> NotificationService consumes
- Telemetry ingestion -> TelemetryService evaluates -> TelemetryAlertEvent published -> NotificationService consumes

Security:
- All protected endpoints require JWT with tenant_id claim and role where applicable.

Deployment hints:
- Each service provides a Dockerfile and Kubernetes manifests under each project's K8s/ folder.
- Use environment variables for connection strings, RabbitMQ credentials, and JWT secrets.
