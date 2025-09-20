using EM.Authentication.Application.Providers;
using EM.Authentication.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using JwtRegisteredClaimNames = System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames;

namespace EM.Authentication.Infraestructure.Providers;

public sealed class TokenProvider(IConfiguration configuration) : ITokenProvider
{
    public string GenerateJwtToken(User user)
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

    public string GenerateJwtRefreshToken(User user)
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

    public DateTime GetJwtTokenExpiration(string accessToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var jsonToken = handler.ReadToken(accessToken) as JwtSecurityToken;

        if (jsonToken == null)
        {
            return default;
        }

        var expirationClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp);

        if (expirationClaim != null && long.TryParse(expirationClaim.Value, out long expTimestamp))
        {
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(expTimestamp);
            return dateTimeOffset.UtcDateTime;
        }

        return default;
    }

    public string GenerateSecurityToken()
    {
        short length = 6;
        char[] Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();
        StringBuilder tokenBuilder = new StringBuilder(length);

        using var rng = RandomNumberGenerator.Create();
        byte[] data = new byte[4];

        for (int i = 0; i < length; i++)
        {
            rng.GetBytes(data);
            uint num = BitConverter.ToUInt32(data, 0);
            tokenBuilder.Append(Chars[num % Chars.Length]);
        }

        return tokenBuilder.ToString();
    }

    private SigningCredentials GetCredentialSecretKey()
    {
        string secretKey = configuration["Jwt:SecretKey"] ?? "";
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
    }
}

