using EM.Authentication.Domain.Entities;
using WH.SharedKernel;

namespace EM.Authentication.Domain;

public interface IUserRepository : IRepository<User>
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task AddTokenAsync(UserToken userToken, CancellationToken cancellationToken);
    void Update(User user);
    void UpdateToken(UserToken userToken);
    Task<User?> GetByEmailAsync(string emailAddress, CancellationToken cancellationToken);
    Task<Profile?> GetProfileByNameAsync(string profileName, CancellationToken cancellationToken);
    Task<UserToken?> GetTokenByIdAsync(Guid id, CancellationToken cancellationToken);
}
