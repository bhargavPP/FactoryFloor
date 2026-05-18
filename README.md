# FactoryFloor

FactoryFloor is a cloud-native manufacturing operations intelligence platform designed for small and mid-size manufacturers.

The platform helps factories:

* monitor machine activity
* track downtime
* manage maintenance schedules
* receive predictive maintenance alerts
* improve operational visibility
* reduce unplanned machine failures

The system is built using:

* .NET microservices
* RabbitMQ
* PostgreSQL
* MongoDB
* Redis
* Docker
* Kubernetes
* OpenTelemetry
* Prometheus + Grafana

---

# Vision

Small manufacturing companies are often underserved by enterprise factory monitoring solutions.

Existing industrial platforms are:

* expensive
* complex
* consultant-heavy
* difficult for SMB manufacturers to adopt

FactoryFloor focuses on providing:

* affordable operational intelligence
* modern cloud-native architecture
* simple onboarding
* scalable infrastructure
* real-time monitoring capabilities

without requiring expensive industrial hardware during the initial adoption phase.

---

# Core Features

## Multi-Tenant SaaS Platform

* Organization-based tenant isolation
* Role-based access control
* JWT authentication
* Secure API access

---

## Machine Registry

Factories can:

* register machines
* define machine types
* assign locations
* configure thresholds
* define maintenance schedules

---

## Telemetry Ingestion

Supports:

* CSV uploads
* manual machine data entry
* simulated telemetry streams
* future IoT/PLC integrations

---

## Maintenance Management

* recurring maintenance schedules
* work order tracking
* maintenance history
* technician assignments
* downtime tracking

---

## Alerting & Notifications

Alerts for:

* overheating
* threshold violations
* maintenance due
* abnormal telemetry
* downtime events

Notification channels:

* email
* Slack
* Teams (future)
* SMS (future)

---

## Predictive Maintenance

The platform will support:

* anomaly detection
* threshold analysis
* predictive failure alerts
* machine health scoring
* usage trend analysis

---

## Observability

Built-in observability stack:

* distributed tracing
* centralized logging
* metrics dashboards
* operational monitoring

Powered by:

* OpenTelemetry
* Prometheus
* Grafana
* Jaeger

---

# High-Level Architecture

```text
FactoryFloor/
└── src/
    ├── gateway/
    │   └── FactoryFloor.Gateway/
    │
    ├── services/
    │   ├── identity-service/
    │   ├── machine-service/
    │   ├── telemetry-service/
    │   ├── maintenance-service/
    │   ├── notification-service/
    │   └── prediction-service/
    │
    └── shared/
        ├── building-blocks/
        └── contracts/
```

---

# Microservices

## Identity Service

Responsibilities:

* authentication
* authorization
* tenant management
* JWT token generation
* RBAC

---

## Machine Service

Responsibilities:

* machine management
* machine metadata
* threshold configuration
* machine status tracking

---

## Telemetry Service

Responsibilities:

* telemetry ingestion
* CSV processing
* threshold evaluation
* event publishing
* anomaly preprocessing

---

## Maintenance Service

Responsibilities:

* maintenance scheduling
* work orders
* technician workflows
* downtime tracking

---

## Notification Service

Responsibilities:

* email alerts
* Slack notifications
* future SMS support
* event-driven notifications

---

## Prediction Service

Responsibilities:

* anomaly detection
* predictive maintenance logic
* machine health analysis
* operational insights

---

# Technology Stack

| Area                | Technology                |
| ------------------- | ------------------------- |
| Backend             | .NET 9                    |
| API                 | ASP.NET Core Minimal APIs |
| Messaging           | RabbitMQ + MassTransit    |
| Relational Database | PostgreSQL                |
| Telemetry Storage   | MongoDB                   |
| Caching             | Redis                     |
| API Gateway         | YARP                      |
| Frontend            | Angular                   |
| Containerization    | Docker                    |
| Orchestration       | Kubernetes                |
| Observability       | OpenTelemetry             |
| Metrics             | Prometheus                |
| Dashboards          | Grafana                   |
| Tracing             | Jaeger                    |

---

# Architectural Principles

## Domain-Driven Design

Each service owns:

* its domain
* its database
* its business rules
* its APIs

---

## Event-Driven Architecture

Services communicate asynchronously using:

* RabbitMQ
* integration events
* background consumers

Example events:

* MachineCreatedEvent
* TelemetryUploadedEvent
* ThresholdExceededEvent
* MaintenanceScheduledEvent

---

## Cloud-Native Design

The platform is designed for:

* horizontal scaling
* containerization
* Kubernetes deployment
* distributed tracing
* resilient workloads

---

# Development Roadmap

## Phase 1

Core foundation:

* Identity Service
* Machine Service
* PostgreSQL setup
* JWT authentication
* Multi-tenancy

---

## Phase 2

Operational workflows:

* Telemetry ingestion
* CSV processing
* Threshold alerts
* Notification Service
* RabbitMQ integration

---

## Phase 3

Maintenance workflows:

* Work orders
* Maintenance scheduling
* Downtime tracking
* Technician workflows

---

## Phase 4

Kubernetes + Observability:

* Dockerization
* Kubernetes manifests
* Helm charts
* OpenTelemetry
* Prometheus + Grafana
* Distributed tracing

---

## Phase 5

Advanced intelligence:

* anomaly detection
* predictive maintenance
* AI-based recommendations
* operational analytics

---

# Local Development

## Prerequisites

Install:

* .NET 9 SDK
* Docker Desktop
* PostgreSQL
* RabbitMQ
* MongoDB
* Redis

Optional:

* Kind
* kubectl
* Helm

---

# Running the Solution

```bash
cd src
```

```bash
dotnet restore
```

```bash
dotnet build
```

Run a service:

```bash
dotnet run --project services/identity-service/FactoryFloor.IdentityService.API
```

---

# Future Goals

Future platform capabilities:

* real-time telemetry streaming
* OPC UA integrations
* Modbus integrations
* edge device support
* mobile dashboards
* AI-based operational optimization
* factory analytics
* multi-factory management

---

# Project Goals

This project is intended to:

* deepen Kubernetes expertise
* master microservice architecture
* build production-grade distributed systems
* learn observability patterns
* implement event-driven communication
* gain real SaaS engineering experience
* build a commercially viable platform

---

# License

This project is currently under active development.
