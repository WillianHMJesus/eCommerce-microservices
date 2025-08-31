using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Domain;

namespace EM.Authentication.Application.Mappers;

public interface IUserMapper
{
    UserResponse Map(User user);
    User Map(AddUserCommand command, string passwordHash);
}
