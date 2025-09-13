namespace EM.Authentication.Application.Providers;

public interface IPasswordProvider
{
    string HashPassword(string password);
    bool VerifyHashedPassword(string hashedPassword, string providedPassword);
}
