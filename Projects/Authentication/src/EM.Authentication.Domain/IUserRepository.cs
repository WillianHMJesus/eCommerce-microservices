using EM.Authentication.Domain.Entities;
using WH.SharedKernel;

namespace EM.Authentication.Domain;

public interface IUserRepository : IRepository<User>
{
    Task AddAsync(User user, CancellationToken cancellationToken);
    void Delete(User user);
    void Update(User user);
    Task<User?> GetByEmailAsync(string emailAddress, CancellationToken cancellationToken);
    Task<Profile?> GetProfileByNameAsync(string profileName, CancellationToken cancellationToken);
}
