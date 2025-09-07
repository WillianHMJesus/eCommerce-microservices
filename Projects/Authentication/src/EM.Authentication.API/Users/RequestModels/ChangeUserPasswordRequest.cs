namespace EM.Authentication.API.Users.RequestModels;

public record ChangeUserPasswordRequest
{
    public string EmailAddress { get; set; } = "";
    public string OldPassword { get; set; } = "";
    public string NewPassword { get; set; } = "";
    public string ConfirmPassword { get; set; } = "";
}
