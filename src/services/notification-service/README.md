# FactoryFloor.NotificationService

Purpose
- Consumes domain events (e.g., MachineCreatedEvent, TelemetryAlertEvent) and performs notification-related actions. Currently logs received events.

Endpoints
- GET /api/health — health check

Config
- RabbitMQ:Host, RabbitMQ:Username, RabbitMQ:Password

Run
- dotnet run --project src/services/notification-service/FactoryFloor.NotificationService

Notes
- Uses MassTransit and a MachineCreatedConsumer implementation to handle events.
