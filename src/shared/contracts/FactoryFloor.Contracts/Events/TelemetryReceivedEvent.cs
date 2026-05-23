using System;
using System.Collections.Generic;
using System.Text;

namespace FactoryFloor.Contracts.Events
{
    public record TelemetryReceivedEvent
    {
        public Guid MachineId { get; init; }
        public Guid TenantId { get; init; }
        public string MetricName { get; init; } = string.Empty;
        public double Value { get; init; }
        public string Unit { get; init; } = string.Empty;
        public DateTime RecordedAt { get; init; } = DateTime.UtcNow;
    }
}
