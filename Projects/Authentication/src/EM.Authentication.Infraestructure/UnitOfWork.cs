using WH.SharedKernel;

namespace EM.Authentication.Infraestructure;

public sealed class UnitOfWork(AuthenticationContext context) : IUnitOfWork
{
    public async Task<bool> CommitAsync(CancellationToken cancellationToken)
    {
        return await context.SaveChangesAsync(cancellationToken) > 0;
    }
}
