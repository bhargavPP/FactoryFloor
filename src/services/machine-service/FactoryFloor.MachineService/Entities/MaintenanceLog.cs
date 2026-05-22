namespace FactoryFloor.MachineService.Entities
{
    public class MaintenanceLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid MachineId { get; set; }
        public Guid TenantId { get; set; }
        public string Description { get; set; } = string.Empty;
        public MaintenanceType Type { get; set; }
        public DateTime PerformedAt { get; set; } = DateTime.UtcNow;
        public string PerformedBy { get; set; } = string.Empty;
        public decimal Cost { get; set; }
        public Machine Machine { get; set; } = null!;
    }
    public enum MaintenanceType
    {
        Preventive,
        Corrective,
        Inspection,
        Emergency
    }
}
