# FactoryFloor.Gateway

Purpose
- Acts as the API gateway and reverse proxy using YARP. Routes incoming requests to downstream services and applies JWT-based authentication.

Key config
- appsettings.json contains ReverseProxy routes and JWT settings:
  - Jwt:Secret, Jwt:Issuer, Jwt:Audience
  - ReverseProxy.Routes and ReverseProxy.Clusters

Run
- dotnet run --project src/gateway/FactoryFloor.Gateway

Notes
- Ensure downstream service DNS names (identity-service, machine-service, telemetry-service) are reachable in your environment.
