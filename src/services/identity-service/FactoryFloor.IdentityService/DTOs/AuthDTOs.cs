namespace FactoryFloor.IdentityService.DTOs;

public record RegisterTenantRequest
(
    string TenantName,
    string TenantSlug,
    string AdminEmail,
    string AdminPassword,
    string FirstName,
    string LastName
);
public record LoginRequest(
    string Email,
    string Password,
    string TenantSlug
    );
public record AuthResponse(
    string AccessToken,
    string Email,
    string Role,
    Guid TenantId,
    DateTime ExpiresAt
    );