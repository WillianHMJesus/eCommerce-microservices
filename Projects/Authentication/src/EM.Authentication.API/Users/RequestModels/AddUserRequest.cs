namespace EM.Authentication.API.Users.RequestModels;

public record AddUserRequest
{
    public string UserName { get; set; } = "";
    public string EmailAddress { get; set; } = "";
    public string Password { get; set; } = "";
    public string ConfirmPassword { get; set; } = "";
    public string ProfileName { get; set; } = "";
}
