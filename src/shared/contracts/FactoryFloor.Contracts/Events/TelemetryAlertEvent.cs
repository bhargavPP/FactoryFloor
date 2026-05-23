using System;
using System.Collections.Generic;
using System.Text;

namespace FactoryFloor.Contracts.Events
{
    public record TelemetryAlertEvent
    {
        public Guid MachineId { get; init; }
        public Guid TenantId { get; init; }
        public string MachineName { get; init; } = string.Empty;
        public string MetricName { get; init; } = string.Empty;
        public double CurrentValue { get; init; }
        public double ThresholdValue { get; init; }
        public string Unit { get; init; } = string.Empty;
        public AlertSeverity Severity { get; init; }
        public DateTime AlertedAt { get; init; } = DateTime.UtcNow;
    }
    public enum AlertSeverity
    {
        Warning,
        Critical
    }
}
