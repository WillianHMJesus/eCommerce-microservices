using EM.Authentication.Application.Commands.AddUser;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;

namespace EM.Authentication.Application.Mappers;

public interface IUserMapper
{
    UserResponse Map(User user);
    User Map(AddUserCommand command, string passwordHash);
    UserToken Map(Guid userId, string tokenHash, short minutesExpire);
}
