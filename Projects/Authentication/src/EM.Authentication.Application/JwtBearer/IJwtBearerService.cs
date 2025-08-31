using EM.Authentication.Domain;

namespace EM.Authentication.Application.JwtBearer;

public interface IJwtBearerService
{
    string GenerateToken(User user);
    string GenerateRefreshToken(User user);
}
