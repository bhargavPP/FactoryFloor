Terminal 1 — watch telemetry logs:
powershellkubectl logs deployment/telemetry-service -f
Terminal 2 — run the test:
powershell# Login
$response = Invoke-RestMethod -Method Post `
  -Uri "http://api.factoryfloor.local/api/auth/login" `
  -ContentType "application/json" `
  -Body '{"email":"admin@acme.com","password":"Admin@123456","tenantSlug":"acme-manufacturing"}'

$token = $response.accessToken
$machineId = "bf53aeab-e888-4426-926b-1d5e93fbab4b" # use a real machineId from your DB

# Step 1 - Set thresholds
Invoke-RestMethod -Method Post `
  -Uri "http://api.factoryfloor.local/api/telemetry/thresholds" `
  -ContentType "application/json" `
  -Headers @{Authorization = "Bearer $token"} `
  -Body "{
    `"machineId`": `"$machineId`",
    `"machineName`": `"CNC Machine 1`",
    `"metricName`": `"Temperature`",
    `"warningThreshold`": 75.0,
    `"criticalThreshold`": 90.0,
    `"unit`": `"C`"
  }"

# Step 2 - Send normal reading (below threshold)
Invoke-RestMethod -Method Post `
  -Uri "http://api.factoryfloor.local/api/telemetry/ingest" `
  -ContentType "application/json" `
  -Headers @{Authorization = "Bearer $token"} `
  -Body "{
    `"machineId`": `"$machineId`",
    `"metricName`": `"Temperature`",
    `"value`": 65.0,
    `"unit`": `"C`"
  }"

# Step 3 - Send warning reading
Invoke-RestMethod -Method Post `
  -Uri "http://api.factoryfloor.local/api/telemetry/ingest" `
  -ContentType "application/json" `
  -Headers @{Authorization = "Bearer $token"} `
  -Body "{
    `"machineId`": `"$machineId`",
    `"metricName`": `"Temperature`",
    `"value`": 78.0,
    `"unit`": `"C`"
  }"

# Step 4 - Send critical reading
Invoke-RestMethod -Method Post `
  -Uri "http://api.factoryfloor.local/api/telemetry/ingest" `
  -ContentType "application/json" `
  -Headers @{Authorization = "Bearer $token"} `
  -Body "{
    `"machineId`": `"$machineId`",
    `"metricName`": `"Temperature`",
    `"value`": 95.0,
    `"unit`": `"C`"
  }"

# Step 5 - Get all readings
Invoke-RestMethod -Method Get `
  -Uri "http://api.factoryfloor.local/api/telemetry/machines/$machineId" `
  -Headers @{Authorization = "Bearer $token"}
In Terminal 1 you should see warning and critical threshold breach logs.