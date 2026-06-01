$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "==================================="
Write-Host "FactoryFloor Rebuild"
Write-Host "==================================="

# ============================================
# CONFIGURATION
# ============================================

$SolutionRoot = "D:\Azure\FactoryFloor\src"

# ============================================
# GO TO SOLUTION ROOT
# ============================================

Set-Location $SolutionRoot

# ============================================
# VERIFY MINIKUBE
# ============================================

Write-Host ""
Write-Host "Checking Minikube..."

$minikubeStatus = minikube status --format "{{.Host}}"

if ($minikubeStatus -ne "Running") {
    throw "Minikube is not running."
}

Write-Host "Minikube is running."

# ============================================
# BUILD GATEWAY
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "Building Gateway"
Write-Host "==================================="

minikube image build `
  -t factoryfloor-gateway:latest `
  -f gateway/FactoryFloor.Gateway/Dockerfile .

if ($LASTEXITCODE -ne 0) {
    throw "Gateway image build failed."
}

# ============================================
# BUILD IDENTITY SERVICE
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "Building Identity Service"
Write-Host "==================================="

minikube image build `
  -t factoryfloor-identity:latest `
  -f services/identity-service/FactoryFloor.IdentityService/Dockerfile .

if ($LASTEXITCODE -ne 0) {
    throw "Identity Service image build failed."
}

# ============================================
# BUILD MACHINE SERVICE
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "Building Machine Service"
Write-Host "==================================="

minikube image build `
  -t factoryfloor-machine:latest `
  -f services/machine-service/FactoryFloor.MachineService/Dockerfile .

if ($LASTEXITCODE -ne 0) {
    throw "Machine Service image build failed."
}

# ============================================
# BUILD NOTIFICATION SERVICE
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "Building Notification Service"
Write-Host "==================================="

minikube image build `
  -t factoryfloor-notification:latest `
  -f services/notification-service/FactoryFloor.NotificationService/Dockerfile .

if ($LASTEXITCODE -ne 0) {
    throw "Notification Service image build failed."
}

# ============================================
# BUILD TELEMETRY SERVICE
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "Building Telemetry Service"
Write-Host "==================================="

minikube image build `
  -t factoryfloor-telemetry:latest `
  -f services/telemetry-service/FactoryFloor.TelemetryService/Dockerfile .

if ($LASTEXITCODE -ne 0) {
    throw "Telemetry Service image build failed."
}

# ============================================
# VERIFY IMAGES
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "Verifying Images"
Write-Host "==================================="

minikube image ls | Select-String "factoryfloor"

# ============================================
# DEPLOY HELM CHART
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "Deploying Helm Chart"
Write-Host "==================================="

Set-Location "$SolutionRoot\infrastructure\helm\factoryfloor"

helm upgrade --install factoryfloor-dev .

if ($LASTEXITCODE -ne 0) {
    throw "Helm deployment failed."
}

# ============================================
# RESTART DEPLOYMENTS
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "Restarting Deployments"
Write-Host "==================================="

kubectl rollout restart deployment api-gateway
kubectl rollout restart deployment identity-service
kubectl rollout restart deployment machine-service
kubectl rollout restart deployment notification-service
kubectl rollout restart deployment telemetry-service

# ============================================
# WAIT FOR ROLLOUT
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "Waiting For Rollouts"
Write-Host "==================================="

kubectl rollout status deployment/api-gateway --timeout=300s
kubectl rollout status deployment/identity-service --timeout=300s
kubectl rollout status deployment/machine-service --timeout=300s
kubectl rollout status deployment/notification-service --timeout=300s
kubectl rollout status deployment/telemetry-service --timeout=300s

# ============================================
# FINAL STATUS
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "Cluster Status"
Write-Host "==================================="

kubectl get pods
kubectl get svc
kubectl get ingress

# ============================================
# GRAFANA INFO
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "Grafana"
Write-Host "==================================="

$grafanaPort = kubectl get svc monitoring-grafana `
  -n monitoring `
  -o jsonpath="{.spec.ports[0].nodePort}"

$minikubeIp = minikube ip

Write-Host ""
Write-Host "Grafana URL:"
Write-Host "http://$($minikubeIp):$grafanaPort"

Write-Host ""
Write-Host "Username: admin"
Write-Host "Password: admin123"

# ============================================
# API URL
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "API Gateway"
Write-Host "==================================="

Write-Host ""
Write-Host "Swagger:"
Write-Host "http://api.factoryfloor.local/swagger"

# ============================================
# COMPLETE
# ============================================

Write-Host ""
Write-Host "==================================="
Write-Host "FactoryFloor Deployment Complete"
Write-Host "==================================="