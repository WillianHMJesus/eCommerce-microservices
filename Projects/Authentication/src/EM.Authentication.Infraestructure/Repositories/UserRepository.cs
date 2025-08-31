using EM.Authentication.Domain;
using EM.Authentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EM.Authentication.Infraestructure.Repositories;

public sealed class UserRepository(AuthenticationContext context) : IUserRepository
{
    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await context.Users.AddAsync(user, cancellationToken);
    }

    public void Delete(User user)
    {
        context.Users.Remove(user);
    }

    public void Update(User user)
    {
        context.Users.Update(user);
    }

    public async Task<User?> GetByEmailAsync(string emailAddress, CancellationToken cancellationToken)
    {
        return await context.Users
            .Include(x => x.Profiles)
            .ThenInclude(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email.EmailAddress == emailAddress, cancellationToken);
    }

    public async Task<Profile?> GetProfileByNameAsync(string profileName, CancellationToken cancellationToken)
    {
        return await context.Profiles
            .FirstOrDefaultAsync(x => x.Name == profileName, cancellationToken);
    }
}
