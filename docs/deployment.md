# Deployment

This document summarizes how to package and deploy FactoryFloor services using Docker and Kubernetes.

Docker images
- Each project contains a Dockerfile targeting .NET 10. Build with:
  - docker build -t factoryfloor/<service-name> -f src/<path-to-service>/Dockerfile .

Kubernetes
- Each service has manifest snippets under src/<service>/K8s/*.yaml.
- Typical manifests include Deployment, Service, Secrets, and optionally HPA and Ingress for the Gateway.

Environment variables and configuration
- Identity Service
  - ConnectionStrings:DefaultConnection -> Postgres connection string
  - Jwt:Secret, Jwt:Issuer, Jwt:Audience
- Machine Service
  - ConnectionStrings:DefaultConnection -> Postgres for machines
  - RabbitMQ:Host, RabbitMQ:Username, RabbitMQ:Password
  - Jwt:Secret/Issuer/Audience
- Telemetry Service
  - MongoDB:ConnectionString, MongoDB:DatabaseName
  - RabbitMQ:Host/Username/Password
  - Jwt:Secret/Issuer/Audience
- Notification Service
  - RabbitMQ:Host/Username/Password

Secrets
- Use Kubernetes Secrets for database passwords and JWT secrets. Example secret manifests exist under each project's K8s folder.

Networking
- The Gateway routes requests to internal services. Ensure DNS or service names (identity-service, machine-service, telemetry-service) resolve in the cluster.

High-level steps to deploy
1. Create namespaces and secrets (postgres, rabbitmq, mongodb credentials, jwt secrets).
2. Deploy databases (or use managed services) and ensure connectivity.
3. Deploy services in dependency order: Identity -> Machine -> Telemetry -> Notification -> Gateway.
4. Verify liveness/health endpoints and check logs.

Rollback & upgrades
- Use standard Kubernetes rolling updates and health checks. Keep stateful databases backed up.

CI/CD
- Build Docker images in pipeline, push to registry, and apply Kubernetes manifests or use Helm charts for templating.
