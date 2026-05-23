namespace FactoryFloor.TelemetryService.DTOs;

public record IngestTelemetryRequest(
    Guid MachineId,
    string MetricName,
    double Value,
    string Unit
);

public record TelemetryReadingResponse(
    string Id,
    Guid MachineId,
    string MetricName,
    double Value,
    string Unit,
    DateTime RecordedAt
);

public record SetThresholdRequest(
    Guid MachineId,
    string MachineName,
    string MetricName,
    double WarningThreshold,
    double CriticalThreshold,
    string Unit
);

public record ThresholdResponse(
    string Id,
    Guid MachineId,
    string MetricName,
    double WarningThreshold,
    double CriticalThreshold,
    string Unit
);