using EM.Authentication.Application.JwtBearer;
using EM.Authentication.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace EM.Authentication.Infraestructure.JwtBearer;

public sealed class JwtBearerService(IConfiguration configuration) : IJwtBearerService
{
    public string GenerateToken(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email.EmailAddress),
                new Claim(ClaimTypes.Name, user.UserName)
            ]),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:ExpirationInMinutes")),
            SigningCredentials = GetCredentialSecretKey()
        };

        user.Profiles.SelectMany(x => x.Roles).ToList().ForEach(x =>
        {
            tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, x.Name.ToString()));
        });

        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }

    public string GenerateRefreshToken(User user)
    {
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
            ]),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:RefreshExpirationInMinutes")),
            SigningCredentials = GetCredentialSecretKey()
        };

        var handler = new JsonWebTokenHandler();

        return handler.CreateToken(tokenDescriptor);
    }

    private SigningCredentials GetCredentialSecretKey()
    {
        string secretKey = configuration["Jwt:SecretKey"] ?? "";
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }
}

