using FactoryFloor.Contracts.Events;
using MassTransit;

namespace FactoryFloor.TelemetryService.Consumers;

public class TelemetryAlertConsumer : IConsumer<TelemetryAlertEvent>
{
    private readonly ILogger<TelemetryAlertConsumer> _logger;

    public TelemetryAlertConsumer(ILogger<TelemetryAlertConsumer> logger)
    {
        _logger = logger;
    }

    public Task Consume(ConsumeContext<TelemetryAlertEvent> context)
    {
        var alert = context.Message;

        _logger.LogWarning("=== TELEMETRY ALERT ===");
        _logger.LogWarning("Severity : {Severity}", alert.Severity);
        _logger.LogWarning("Machine  : {MachineName}", alert.MachineName);
        _logger.LogWarning("Metric   : {MetricName}", alert.MetricName);
        _logger.LogWarning("Value    : {Value}{Unit}", alert.CurrentValue, alert.Unit);
        _logger.LogWarning("Threshold: {Threshold}{Unit}", alert.ThresholdValue, alert.Unit);
        _logger.LogWarning("Time     : {Time}", alert.AlertedAt);
        _logger.LogWarning("======================");

        return Task.CompletedTask;
    }
}