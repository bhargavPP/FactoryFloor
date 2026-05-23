Create chart
helm create factoryfloor
Validate
helm lint factoryfloor
Simulate deployment
helm install test ./factoryfloor --dry-run --debug
Install
helm install factoryfloor-dev ./factoryfloor
Update
helm upgrade factoryfloor-dev ./factoryfloor