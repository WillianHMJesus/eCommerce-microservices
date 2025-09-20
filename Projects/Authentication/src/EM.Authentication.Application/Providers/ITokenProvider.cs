using EM.Authentication.Domain;

namespace EM.Authentication.Application.Providers;

public interface ITokenProvider
{
    string GenerateJwtToken(User user);
    string GenerateJwtRefreshToken(User user);
    DateTime GetJwtTokenExpiration(string accessToken);
    string GenerateSecurityToken();
}
