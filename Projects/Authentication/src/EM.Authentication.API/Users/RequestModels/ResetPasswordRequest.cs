namespace EM.Authentication.API.Users.RequestModels;

public sealed record ResetPasswordRequest
{
    public Guid UserTokenId { get; set; }
    public string NewPassword { get; set; } = "";
    public string ConfirmPassword { get; set; } = "";
}
