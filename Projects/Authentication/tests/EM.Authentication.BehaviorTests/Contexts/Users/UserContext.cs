using EM.Authentication.API.Users.RequestModels;

namespace EM.Authentication.BehaviorTests.Contexts.Users;

public class UserContext
{
    public AddUserRequest? AddUserRequest { get; set; }
    public ChangeUserPasswordRequest? ChangeUserPasswordRequest { get; set; }
    public string? AccessToken { get; set; }
    public HttpResponseMessage? Response { get; set; }
}
