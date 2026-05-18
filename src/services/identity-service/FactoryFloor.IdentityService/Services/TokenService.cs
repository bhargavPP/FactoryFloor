using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FactoryFloor.IdentityService.Entities;
using Microsoft.IdentityModel.Tokens;
namespace FactoryFloor.IdentityService.Services;

public interface ITokenService
{
    string GenerateToken(User user);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    public TokenService(IConfiguration config)
    {
        _config = config??throw new ArgumentNullException(nameof(config));
    }
    public string GenerateToken(User user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]));

        var claims = new[]
        {
              new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("tenant_id", user.TenantId.ToString()),
            new Claim("first_name", user.FirstName)
        };

        var token = new JwtSecurityToken(
             issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256)
            );
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}