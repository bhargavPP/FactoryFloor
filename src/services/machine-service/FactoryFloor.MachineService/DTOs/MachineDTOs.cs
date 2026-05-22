namespace FactoryFloor.MachineService.DTOs
{
    public record CreateMachineRequest(
    string Name,
    string SerialNumber,
    string Model,
    string Manufacturer,
    string Location,
    DateTime InstalledAt
);

    public record UpdateMachineRequest(
        string Name,
        string Location,
        string Status
    );

    public record MachineResponse(
    Guid Id,
    string Name,
    string SerialNumber,
    string Model,
    string Manufacturer,
    string Location,
    string Status,
    DateTime InstalledAt,
    DateTime? LastMaintenanceAt,
    DateTime? NextMaintenanceAt
);
    public record CreateMaintenanceLogRequest(
    Guid MachineId,
    string Description,
    string Type,
    string PerformedBy,
    decimal Cost
);
    public record MaintenanceLogResponse(
    Guid Id,
    Guid MachineId,
    string Description,
    string Type,
    DateTime PerformedAt,
    string PerformedBy,
    decimal Cost
);
}
