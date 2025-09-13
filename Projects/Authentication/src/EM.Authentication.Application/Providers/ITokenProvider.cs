using EM.Authentication.Domain;

namespace EM.Authentication.Application.Providers;

public interface ITokenProvider
{
    string Generate(User user);
    string GenerateRefreshToken(User user);
    DateTime GetTokenExpiration(string accessToken);
}
