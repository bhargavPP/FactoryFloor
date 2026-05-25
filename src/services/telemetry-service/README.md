# FactoryFloor.TelemetryService

Purpose
- Ingest telemetry readings, store them in MongoDB, evaluate machine thresholds, and publish TelemetryAlertEvent when thresholds are breached.

Endpoints (all under /api/telemetry and require JWT)
- POST /api/telemetry/ingest — ingest a telemetry reading
- GET /api/telemetry/machines/{machineId} — get recent readings
- POST /api/telemetry/thresholds — set thresholds for a machine metric
- GET /api/telemetry/thresholds/{machineId} — get thresholds
- GET /api/health — health check

Config
- MongoDB:ConnectionString, MongoDB:DatabaseName
- RabbitMQ:Host/Username/Password
- Jwt:Secret/Issuer/Audience

Run
- dotnet run --project src/services/telemetry-service/FactoryFloor.TelemetryService

Notes
- See README.txt for a PowerShell example to test ingestion and threshold alerts.
