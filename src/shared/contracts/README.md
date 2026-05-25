# FactoryFloor.Contracts

Purpose
- Shared contract types (events/DTOs) used across services, such as MachineCreatedEvent and TelemetryAlertEvent.

Key types
- MachineCreatedEvent: published by Machine Service when a machine is created.
- TelemetryAlertEvent: published by Telemetry Service when thresholds are breached.

Usage
- Reference this project from services that publish/consume these events to keep event contracts consistent.
