namespace FactoryFloor.Contracts;

public record MachineCreatedEvent
{
    public Guid MachineId { get; init; }
    public Guid TenantId { get; init; }
    public string MachineName { get; init; } = string.Empty;
    public string SerialNumber { get; init; } = string.Empty;
    public string Location { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}