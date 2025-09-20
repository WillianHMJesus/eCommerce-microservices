namespace EM.Authentication.API.Users.RequestModels;

public sealed record SendUserTokenRequest
{
    public string EmailAddress { get; set; } = "";
}
