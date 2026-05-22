namespace FactoryFloor.MachineService.Entities
{
    public class Machine
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TenantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public string Manufacturer { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public MachineStatus Status { get; set; } = MachineStatus.Active;
        public DateTime InstalledAt { get; set; }
        public DateTime? LastMaintenanceAt { get; set; }
        public DateTime? NextMaintenanceAt { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<MaintenanceLog> MaintenanceLogs { get; set; } = new List<MaintenanceLog>();
    }

    public enum MachineStatus
    {
        Active,
        Inactive,
        UnderMaintenance,
        Decommissioned
    }
}
