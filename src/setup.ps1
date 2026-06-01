# ============================================
# FactoryFloor Production-Style Bootstrap
# ============================================

$ErrorActionPreference = "Stop"

Write-Host ""
Write-Host "==========================================="
Write-Host "FactoryFloor Platform Bootstrap"
Write-Host "==========================================="
Write-Host ""

# ============================================
# CONFIGURATION
# ============================================

$MinikubeProfile = "minikube"
$HyperVSwitch = "minikube-switch"

$SolutionRoot = "D:\Azure\FactoryFloor\src"

$HelmChartPath = "$SolutionRoot\infrastructure\helm\factoryfloor"

$GrafanaPassword = "admin123"

# ============================================
# HELPER FUNCTIONS
# ============================================

function Write-Step {
    param([string]$Message)

    Write-Host ""
    Write-Host "=================================================="
    Write-Host $Message
    Write-Host "=================================================="
}

function Test-Command {
    param([string]$Command)

    if (-not (Get-Command $Command -ErrorAction SilentlyContinue)) {
        throw "$Command is not installed or not available in PATH."
    }
}

function Wait-ForPods {
    param(
        [string]$Namespace = "default",
        [int]$TimeoutSeconds = 600
    )

    Write-Host "Waiting for pods in namespace '$Namespace'..."

    $start = Get-Date

    while ($true) {

        $pods = kubectl get pods -n $Namespace --no-headers 2>$null

        if (-not $pods) {
            Start-Sleep -Seconds 5
            continue
        }

        $notReady = $pods | Where-Object {
            ($_ -match "0/") -or
            ($_ -match "ContainerCreating") -or
            ($_ -match "Pending") -or
            ($_ -match "CrashLoopBackOff")
        }

        if (-not $notReady) {
            Write-Host "All pods ready in namespace '$Namespace'"
            break
        }

        $elapsed = (Get-Date) - $start

        if ($elapsed.TotalSeconds -gt $TimeoutSeconds) {
            throw "Timeout waiting for pods in namespace '$Namespace'"
        }

        Start-Sleep -Seconds 10
    }
}

function Build-Image {
    param(
        [string]$ImageName,
        [string]$Dockerfile
    )

    Write-Host ""
    Write-Host "Building image: $ImageName"

    minikube image build `
        -t $ImageName `
        -f $Dockerfile `
        $SolutionRoot

    if ($LASTEXITCODE -ne 0) {
        throw "Failed to build image: $ImageName"
    }
}

# ============================================
# VERIFY TOOLS
# ============================================

Write-Step "Verifying Required Tools"

Test-Command "minikube"
Test-Command "kubectl"
Test-Command "helm"

Write-Host "All required tools found."

# ============================================
# VERIFY HYPER-V SWITCH
# ============================================

Write-Step "Checking Hyper-V Virtual Switch"

$switch = Get-VMSwitch -Name $HyperVSwitch -ErrorAction SilentlyContinue

if (-not $switch) {
    throw "Hyper-V switch '$HyperVSwitch' not found."
}

Write-Host "Hyper-V switch found."

# ============================================
# START MINIKUBE
# ============================================

Write-Step "Checking Minikube Status"

$minikubeRunning = $false

try {
    $status = minikube status --profile $MinikubeProfile --format "{{.Host}}"

    if ($status -eq "Running") {
        $minikubeRunning = $true
    }
}
catch {
    $minikubeRunning = $false
}

if (-not $minikubeRunning) {

    Write-Host "Starting Minikube..."

    minikube start `
        --profile=$MinikubeProfile `
        --driver=hyperv `
        --hyperv-virtual-switch=$HyperVSwitch

    if ($LASTEXITCODE -ne 0) {
        throw "Failed to start Minikube."
    }
}
else {
    Write-Host "Minikube already running."
}

# ============================================
# VERIFY CLUSTER
# ============================================

Write-Step "Verifying Kubernetes Cluster"

kubectl get nodes

if ($LASTEXITCODE -ne 0) {
    throw "Kubernetes cluster not reachable."
}

# ============================================
# ENABLE ADDONS
# ============================================

Write-Step "Enabling Minikube Addons"

minikube addons enable ingress
minikube addons enable metrics-server

# ============================================
# WAIT FOR SYSTEM PODS
# ============================================

Write-Step "Waiting For Kubernetes System Pods"

Wait-ForPods -Namespace "kube-system"

# ============================================
# BUILD MICROSERVICE IMAGES
# ============================================

Write-Step "Building FactoryFloor Images"

Set-Location $SolutionRoot

Build-Image `
    -ImageName "factoryfloor-gateway:latest" `
    -Dockerfile "gateway/FactoryFloor.Gateway/Dockerfile"

Build-Image `
    -ImageName "factoryfloor-identity:latest" `
    -Dockerfile "services/identity/FactoryFloor.IdentityService/Dockerfile"

Build-Image `
    -ImageName "factoryfloor-machine:latest" `
    -Dockerfile "services/machines/FactoryFloor.MachineService/Dockerfile"

Build-Image `
    -ImageName "factoryfloor-telemetry:latest" `
    -Dockerfile "services/telemetry/FactoryFloor.TelemetryService/Dockerfile"

Build-Image `
    -ImageName "factoryfloor-notification:latest" `
    -Dockerfile "services/notifications/FactoryFloor.NotificationService/Dockerfile"

# ============================================
# DEPLOY HELM PLATFORM
# ============================================

Write-Step "Deploying FactoryFloor Platform"

Set-Location $HelmChartPath

helm upgrade --install factoryfloor-dev .

if ($LASTEXITCODE -ne 0) {
    throw "Helm deployment failed."
}

# ============================================
# WAIT FOR APPLICATION PODS
# ============================================

Write-Step "Waiting For Application Pods"

Wait-ForPods -Namespace "default"

# ============================================
# INSTALL MONITORING
# ============================================

Write-Step "Installing Monitoring Stack"

helm repo add prometheus-community https://prometheus-community.github.io/helm-charts 2>$null
helm repo add grafana https://grafana.github.io/helm-charts 2>$null

helm repo update

$monitoringExists = kubectl get namespace monitoring --ignore-not-found

if (-not $monitoringExists) {
    kubectl create namespace monitoring
}

$monitoringRelease = helm list -n monitoring -q | Select-String "monitoring"

if (-not $monitoringRelease) {

    Write-Host "Installing monitoring stack..."

    helm install monitoring `
        prometheus-community/kube-prometheus-stack `
        --namespace monitoring `
        --set prometheus.prometheusSpec.scrapeInterval="15s" `
        --set grafana.adminPassword=$GrafanaPassword `
        --set grafana.service.type=NodePort
}
else {

    Write-Host "Upgrading monitoring stack..."

    helm upgrade monitoring `
        prometheus-community/kube-prometheus-stack `
        --namespace monitoring `
        --set prometheus.prometheusSpec.scrapeInterval="15s" `
        --set grafana.adminPassword=$GrafanaPassword `
        --set grafana.service.type=NodePort
}

# ============================================
# WAIT FOR MONITORING PODS
# ============================================

Write-Step "Waiting For Monitoring Pods"

Wait-ForPods -Namespace "monitoring"

# ============================================
# SHOW PLATFORM STATUS
# ============================================

Write-Step "Cluster Status"

kubectl get pods -A

Write-Step "Services"

kubectl get svc -A

Write-Step "Ingress"

kubectl get ingress

# ============================================
# GRAFANA INFO
# ============================================

Write-Step "Grafana Information"

$grafanaSvc = kubectl get svc monitoring-grafana `
    -n monitoring `
    -o jsonpath="{.spec.ports[0].nodePort}"

$minikubeIp = minikube ip

Write-Host ""
Write-Host "Grafana URL:"
Write-Host "http://$($minikubeIp):$grafanaSvc"
Write-Host ""

Write-Host "Username: admin"
Write-Host "Password: $GrafanaPassword"

# ============================================
# HOSTS FILE INFO
# ============================================

Write-Step "Hosts File Configuration"

Write-Host ""
Write-Host "Add this to:"
Write-Host "C:\Windows\System32\drivers\etc\hosts"
Write-Host ""

Write-Host "$minikubeIp api.factoryfloor.local"

# ============================================
# COMPLETE
# ============================================

Write-Step "Bootstrap Completed Successfully"

Write-Host ""
Write-Host "FactoryFloor platform is running."
Write-Host ""