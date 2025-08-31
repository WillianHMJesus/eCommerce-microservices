using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Domain;

namespace EM.Authentication.Application.Mappers;

public sealed class UserMapper : IUserMapper
{
    public UserResponse Map(User user)
    {
        return new UserResponse
        {
            UserName = user.UserName,
            EmailAddress = user.Email.EmailAddress
        };
    }

    public User Map(AddUserCommand command, string passwordHash)
    {
        return new User(command.UserName, command.EmailAddress, passwordHash);
    }
}
