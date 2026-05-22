using FactoryFloor.MachineService.Data;
using FactoryFloor.MachineService.DTOs;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FactoryFloor.MachineService.Endpoints
{
    public static class MachineEndpoints
    {
        public static void MapMachineEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/api/machines").WithTags("Machines").RequireAuthorization();
            group.MapGet("/", GetAllMachines);
            group.MapGet("/{id:guid}", GetMachine);
            group.MapPost("/", CreateMachine);
            group.MapPut("/{id:guid}", UpdateMachine);
            group.MapDelete("/{id:guid}", DeleteMachine);
            group.MapPost("/{id:guid}/maintenance", AddMaintenanceLog);
            group.MapGet("/{id:guid}/maintenance", GetMaintenanceLogs);

            app.MapGet("/api/health", () => Results.Ok("Machine Service is healthy"));

        }

        private static Guid GetTenantId(ClaimsPrincipal user)
        {
            var tenantIdClaim = user.FindFirst("tenant_id");
            if (tenantIdClaim == null || !Guid.TryParse(tenantIdClaim.Value, out var tenantId))
            {
                throw new UnauthorizedAccessException("Tenant ID claim is missing or invalid.");
            }
            return tenantId;
        }

        private static async Task<IResult> GetAllMachines(MachineDbContext db, ClaimsPrincipal user)
        {
            var tenantId = GetTenantId(user);
            var machines = await db.Machines
                 .AsNoTracking()
                .Where(m => m.TenantId == tenantId)
                .Select(m => new MachineResponse(
                    m.Id, m.Name, m.SerialNumber, m.Model,
                    m.Manufacturer, m.Location, m.Status.ToString(),
                    m.InstalledAt, m.LastMaintenanceAt, m.NextMaintenanceAt))
                .ToListAsync();

            return Results.Ok(machines);
        }
        private static async Task<IResult> GetMachine(Guid id, MachineDbContext db, ClaimsPrincipal user)
        {
            var tenantId = GetTenantId(user);
            var machine = await db.Machines
                                  .AsNoTracking()
                                  .FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);
            return machine is null
           ? Results.NotFound()
           : Results.Ok(new MachineResponse(
               machine.Id, machine.Name, machine.SerialNumber,
               machine.Model, machine.Manufacturer, machine.Location,
               machine.Status.ToString(), machine.InstalledAt,
               machine.LastMaintenanceAt, machine.NextMaintenanceAt));
        }
        private static async Task<IResult> CreateMachine(CreateMachineRequest request, MachineDbContext db, ClaimsPrincipal user)
        {
            var tenantId = GetTenantId(user);
            if (await db.Machines.AnyAsync(m =>
            m.TenantId == tenantId &&
            m.SerialNumber == request.SerialNumber))
                return Results.Conflict("Serial number already exists.");
            var machine = new Entities.Machine
            {
                TenantId = tenantId,
                Name = request.Name,
                SerialNumber = request.SerialNumber,
                Model = request.Model,
                Manufacturer = request.Manufacturer,
                Location = request.Location,
                InstalledAt = request.InstalledAt
            };
            db.Machines.Add(machine);
            await db.SaveChangesAsync();
            return Results.Created($"/api/machines/{machine.Id}", new MachineResponse(
                machine.Id, machine.Name, machine.SerialNumber,
                machine.Model, machine.Manufacturer, machine.Location,
                machine.Status.ToString(), machine.InstalledAt,
                machine.LastMaintenanceAt, machine.NextMaintenanceAt));
        }

        private static async Task<IResult> UpdateMachine(Guid id, UpdateMachineRequest request, MachineDbContext db, ClaimsPrincipal user)
        {
            var tenantId = GetTenantId(user);
            var machine = await db.Machines.FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);
            if (machine is null) return Results.NotFound();
            machine.Name = request.Name;
            machine.Location = request.Location;
            if (Enum.TryParse<Entities.MachineStatus>(request.Status, true, out var status))
                machine.Status = status;
            await db.SaveChangesAsync();
            return Results.NoContent();
        }
        private static async Task<IResult> DeleteMachine(Guid id, MachineDbContext db, ClaimsPrincipal user)
        {
            var tenantId = GetTenantId(user);
            var machine = await db.Machines.FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);
            if (machine is null) return Results.NotFound();
            db.Machines.Remove(machine);
            await db.SaveChangesAsync();
            return Results.NoContent();
        }

        private static async Task<IResult> AddMaintenanceLog(Guid id, CreateMaintenanceLogRequest request, MachineDbContext db, ClaimsPrincipal user)
        {
            var tenantId = GetTenantId(user);
            var machine = await db.Machines.FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);
            if (machine is null) return Results.NotFound();
            var log = new Entities.MaintenanceLog
            {
                MachineId = id,
                TenantId = tenantId,
                Description = request.Description,
                Type = Enum.TryParse<Entities.MaintenanceType>(request.Type, true, out var type) ? type : Entities.MaintenanceType.Preventive,
                PerformedBy = request.PerformedBy,
                Cost = request.Cost
            };

            db.MaintenanceLogs.Add(log);
            machine.LastMaintenanceAt = log.PerformedAt;
            machine.NextMaintenanceAt = log.PerformedAt.AddMonths(6); // Example: next maintenance in 6 months
            await db.SaveChangesAsync();
            return Results.Created($"/api/machines/{id}/maintenance/{log.Id}", new MaintenanceLogResponse(
                log.Id, log.MachineId, log.Description, log.Type.ToString(),
                log.PerformedAt, log.PerformedBy, log.Cost));
        }
        private static async Task<IResult> GetMaintenanceLogs(Guid id, MachineDbContext db, ClaimsPrincipal user)
        {
            var tenantId = GetTenantId(user);
            var machine = await db.Machines.FirstOrDefaultAsync(m => m.Id == id && m.TenantId == tenantId);
            if (machine is null) return Results.NotFound();
            var logs = await db.MaintenanceLogs
                               .AsNoTracking()
                               .Where(ml => ml.MachineId == id && ml.TenantId == tenantId)
                               .ToListAsync();
            var response = logs.Select(log => new MaintenanceLogResponse(
                log.Id, log.MachineId, log.Description, log.Type.ToString(),
                log.PerformedAt, log.PerformedBy, log.Cost));
            return Results.Ok(response);
        }
    }
}
