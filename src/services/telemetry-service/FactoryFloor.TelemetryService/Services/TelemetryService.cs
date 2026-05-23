using FactoryFloor.Contracts.Events;
using FactoryFloor.TelemetryService.DTOs;
using FactoryFloor.TelemetryService.Entities;
using MassTransit;
using MongoDB.Driver;

namespace FactoryFloor.TelemetryService.Services;

public interface ITelemetryService
{
    Task IngestAsync(IngestTelemetryRequest request, Guid tenantId);
    Task<List<TelemetryReadingResponse>> GetReadingsAsync(Guid machineId, Guid tenantId, int limit = 100);
    Task SetThresholdAsync(SetThresholdRequest request, Guid tenantId);
    Task<List<ThresholdResponse>> GetThresholdsAsync(Guid machineId, Guid tenantId);
}

public class TelemetryServiceImpl : ITelemetryService
{
    private readonly IMongoCollection<TelemetryReading> _readings;
    private readonly IMongoCollection<MachineThreshold> _thresholds;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<TelemetryServiceImpl> _logger;

    public TelemetryServiceImpl(
        IMongoDatabase database,
        IPublishEndpoint publishEndpoint,
        ILogger<TelemetryServiceImpl> logger)
    {
        _readings = database.GetCollection<TelemetryReading>("telemetry_readings");
        _thresholds = database.GetCollection<MachineThreshold>("machine_thresholds");
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task IngestAsync(IngestTelemetryRequest request, Guid tenantId)
    {
        // Save reading
        var reading = new TelemetryReading
        {
            MachineId = request.MachineId,
            TenantId = tenantId,
            MetricName = request.MetricName,
            Value = request.Value,
            Unit = request.Unit,
            RecordedAt = DateTime.UtcNow
        };

        await _readings.InsertOneAsync(reading);

        _logger.LogInformation("Telemetry ingested: Machine={MachineId} {MetricName}={Value}{Unit}",
            request.MachineId, request.MetricName, request.Value, request.Unit);

        // Publish event
        await _publishEndpoint.Publish(new TelemetryReceivedEvent
        {
            MachineId = request.MachineId,
            TenantId = tenantId,
            MetricName = request.MetricName,
            Value = request.Value,
            Unit = request.Unit
        });

        // Check thresholds
        await CheckThresholdsAsync(request, tenantId);
    }

    private async Task CheckThresholdsAsync(IngestTelemetryRequest request, Guid tenantId)
    {
        var threshold = await _thresholds
            .Find(t => t.MachineId == request.MachineId &&
                       t.TenantId == tenantId &&
                       t.MetricName == request.MetricName)
            .FirstOrDefaultAsync();

        if (threshold is null) return;

        AlertSeverity? severity = null;

        if (request.Value >= threshold.CriticalThreshold)
            severity = AlertSeverity.Critical;
        else if (request.Value >= threshold.WarningThreshold)
            severity = AlertSeverity.Warning;

        if (severity.HasValue)
        {
            _logger.LogWarning("THRESHOLD BREACHED: Machine={MachineId} {MetricName}={Value}{Unit} Severity={Severity}",
                request.MachineId, request.MetricName, request.Value, request.Unit, severity);

            await _publishEndpoint.Publish(new TelemetryAlertEvent
            {
                MachineId = request.MachineId,
                TenantId = tenantId,
                MachineName = threshold.MachineName,
                MetricName = request.MetricName,
                CurrentValue = request.Value,
                ThresholdValue = severity == AlertSeverity.Critical
                    ? threshold.CriticalThreshold
                    : threshold.WarningThreshold,
                Unit = request.Unit,
                Severity = severity.Value
            });
        }
    }

    public async Task<List<TelemetryReadingResponse>> GetReadingsAsync(
        Guid machineId, Guid tenantId, int limit = 100)
    {
        var readings = await _readings
            .Find(r => r.MachineId == machineId && r.TenantId == tenantId)
            .SortByDescending(r => r.RecordedAt)
            .Limit(limit)
            .ToListAsync();

        return readings.Select(r => new TelemetryReadingResponse(
            r.Id, r.MachineId, r.MetricName, r.Value, r.Unit, r.RecordedAt))
            .ToList();
    }

    public async Task SetThresholdAsync(SetThresholdRequest request, Guid tenantId)
    {
        var filter = Builders<MachineThreshold>.Filter.Where(t =>
            t.MachineId == request.MachineId &&
            t.TenantId == tenantId &&
            t.MetricName == request.MetricName);

        var threshold = new MachineThreshold
        {
            MachineId = request.MachineId,
            TenantId = tenantId,
            MachineName = request.MachineName,
            MetricName = request.MetricName,
            WarningThreshold = request.WarningThreshold,
            CriticalThreshold = request.CriticalThreshold,
            Unit = request.Unit
        };

        await _thresholds.ReplaceOneAsync(filter, threshold,
            new ReplaceOptions { IsUpsert = true });

        _logger.LogInformation("Threshold set: Machine={MachineId} {MetricName} Warning={Warning} Critical={Critical}",
            request.MachineId, request.MetricName,
            request.WarningThreshold, request.CriticalThreshold);
    }

    public async Task<List<ThresholdResponse>> GetThresholdsAsync(
        Guid machineId, Guid tenantId)
    {
        var thresholds = await _thresholds
            .Find(t => t.MachineId == machineId && t.TenantId == tenantId)
            .ToListAsync();

        return thresholds.Select(t => new ThresholdResponse(
            t.Id, t.MachineId, t.MetricName,
            t.WarningThreshold, t.CriticalThreshold, t.Unit))
            .ToList();
    }
}