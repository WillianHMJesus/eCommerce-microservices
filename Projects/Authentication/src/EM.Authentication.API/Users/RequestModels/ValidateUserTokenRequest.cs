namespace EM.Authentication.API.Users.RequestModels;

public sealed record ValidateUserTokenRequest
{
    public Guid UserTokenId { get; set; }
    public string Token { get; set; } = "";
}
