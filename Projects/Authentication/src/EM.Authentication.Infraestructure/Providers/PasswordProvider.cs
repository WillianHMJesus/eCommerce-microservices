using EM.Authentication.Application.Providers;
using Microsoft.AspNetCore.Identity;

namespace EM.Authentication.Infraestructure.Providers;

public sealed class PasswordProvider(IPasswordHasher<PasswordProvider> passwordHasher) : IPasswordProvider
{
    public string HashPassword(string password)
    {
        return passwordHasher.HashPassword(this, password);
    }

    public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
    {
        var passwordVerificationResult = passwordHasher.VerifyHashedPassword(this, hashedPassword, providedPassword);

        return passwordVerificationResult is PasswordVerificationResult.Success;
    }
}
