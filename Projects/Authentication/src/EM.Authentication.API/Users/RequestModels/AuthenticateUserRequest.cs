namespace EM.Authentication.API.Users.RequestModels;

public class AuthenticateUserRequest
{
    public string EmailAddress { get; set; } = "";
    public string Password { get; set; } = "";
}
