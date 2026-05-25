using BCrypt.Net;
using FactoryFloor.IdentityService.Data;
using FactoryFloor.IdentityService.DTOs;
using FactoryFloor.IdentityService.Entities;
using FactoryFloor.IdentityService.Services;
using Microsoft.EntityFrameworkCore;

namespace FactoryFloor.IdentityService.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndPoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth").WithTags("Auth");

        group.MapPost("/register", RegisterTenant);
        group.MapPost("/login", Login);
        group.MapGet("/health", () => Results.Ok("Identity Service is healthy"));
    }

    private static async Task<IResult> RegisterTenant(RegisterTenantRequest request,IdentityDbContext db,ITokenService tokenService)
    {
        // Check slug is unique
        if (await db.Tenants.AnyAsync(t => t.Slug.ToLower() == request.TenantSlug.ToLower()))
            return Results.Conflict("Tenant slug already exists.");
        // Check email is unique
        if (await db.Users.AnyAsync(u => u.Email.ToLower() == request.AdminEmail.ToLower()))
            return Results.Conflict("Email already registered.");

        var tenant = new Tenant
        {
            Name = request.TenantName,
            Slug = request.TenantSlug
        };
        var admin = new User
        {
            TenantId = tenant.Id,
            Email = request.AdminEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.AdminPassword),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = "Admin"
        };

        db.Tenants.Add(tenant);
        db.Users.Add(admin);

        await db.SaveChangesAsync();

        var token = tokenService.GenerateToken(admin);

        return Results.Ok(new AuthResponse(
           token,
           admin.Email,
           admin.Role,
           tenant.Id,
           DateTime.UtcNow.AddHours(8)
       ));
    }
    private static async Task<IResult> Login(
        LoginRequest request,
        IdentityDbContext db,
        ITokenService tokenService)
    {
        var tenant = await db.Tenants
            .FirstOrDefaultAsync(t => t.Slug.ToLower() == request.TenantSlug.ToLower());

        if (tenant is null)
            return Results.NotFound("Tenant not found.");

        var user = await db.Users
            .FirstOrDefaultAsync(u =>
                u.Email == request.Email &&
                u.TenantId == tenant.Id &&
                u.IsActive);

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Results.Unauthorized();

        var token = tokenService.GenerateToken(user);

        return Results.Ok(new AuthResponse(
            token,
            user.Email,
            user.Role,
            tenant.Id,
            DateTime.UtcNow.AddHours(8)
        ));
    }
}