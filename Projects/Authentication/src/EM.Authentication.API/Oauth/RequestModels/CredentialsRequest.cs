namespace EM.Authentication.API.Oauth.RequestModels;

public class CredentialsRequest
{
    public string EmailAddress { get; set; } = "";
    public string Password { get; set; } = "";
}
