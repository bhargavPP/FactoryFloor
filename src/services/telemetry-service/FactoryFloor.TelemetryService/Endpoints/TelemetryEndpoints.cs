using FactoryFloor.TelemetryService.DTOs;
using FactoryFloor.TelemetryService.Services;
using System.Security.Claims;

namespace FactoryFloor.TelemetryService.Endpoints;

public static class TelemetryEndpoints
{
    public static void MapTelemetryEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/telemetry")
            .WithTags("Telemetry")
            .RequireAuthorization();

        group.MapPost("/ingest", IngestTelemetry);
        group.MapGet("/machines/{machineId:guid}", GetReadings);
        group.MapPost("/thresholds", SetThreshold);
        group.MapGet("/thresholds/{machineId:guid}", GetThresholds);

        app.MapGet("/api/health", () => Results.Ok("Telemetry Service is healthy"));
    }

    private static Guid GetTenantId(ClaimsPrincipal user) =>
        Guid.Parse(user.FindFirstValue("tenant_id")!);

    private static async Task<IResult> IngestTelemetry(
        IngestTelemetryRequest request,
        ITelemetryService telemetryService,
        ClaimsPrincipal user)
    {
        await telemetryService.IngestAsync(request, GetTenantId(user));
        return Results.Ok("Telemetry ingested.");
    }

    private static async Task<IResult> GetReadings(
        Guid machineId,
        ITelemetryService telemetryService,
        ClaimsPrincipal user)
    {
        var readings = await telemetryService.GetReadingsAsync(machineId, GetTenantId(user));
        return Results.Ok(readings);
    }

    private static async Task<IResult> SetThreshold(
        SetThresholdRequest request,
        ITelemetryService telemetryService,
        ClaimsPrincipal user)
    {
        await telemetryService.SetThresholdAsync(request, GetTenantId(user));
        return Results.Ok("Threshold set.");
    }

    private static async Task<IResult> GetThresholds(
        Guid machineId,
        ITelemetryService telemetryService,
        ClaimsPrincipal user)
    {
        var thresholds = await telemetryService.GetThresholdsAsync(machineId, GetTenantId(user));
        return Results.Ok(thresholds);
    }
}