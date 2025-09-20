namespace EM.Authentication.Application;

public sealed record UserResponse
{
    public string UserName { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string AccessToken { get; set; } = "";
    public DateTime TokenExpiration { get; set; }
    public string RefreshToken { get; set; } = "";
}
