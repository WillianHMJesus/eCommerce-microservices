using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Infraestructure.Persistense.Write;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly CatalogContext _writeContext;

    public UnitOfWork(CatalogContext writeContext)
        => _writeContext = writeContext;

    public async Task<bool> CommitAsync(CancellationToken cancellationToken)
    {
        return await _writeContext.SaveChangesAsync(cancellationToken) > 0;
    }
}
