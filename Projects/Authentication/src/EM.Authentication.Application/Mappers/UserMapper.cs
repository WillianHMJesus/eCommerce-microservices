using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using WH.SimpleMapper;

namespace EM.Authentication.Application.Mappers;

public sealed class UserMapper :
    ITypeMapper<User, UserResponse>,
    ITypeMapper<(AddUserCommand Command, string PasswordHash), User>,
    ITypeMapper<(Guid UserId, string TokenHash, short MinutesExpire), UserToken>
{
    public UserResponse Map(User user)
    {
        return new UserResponse
        {
            UserName = user.UserName,
            EmailAddress = user.Email.EmailAddress
        };
    }

    public User Map((AddUserCommand Command, string PasswordHash) source)
    {
        return new User(source.Command.UserName, source.Command.EmailAddress, source.PasswordHash);
    }

    public UserToken Map((Guid UserId, string TokenHash, short MinutesExpire) source)
    {
        return new UserToken(
            source.UserId,
            source.TokenHash,
            DateTime.Now,
            DateTime.Now.AddMinutes(source.MinutesExpire)
        );
    }
}
