using EM.Catalog.Application.Interfaces;

namespace EM.Catalog.Infraestructure.Persistense.Write;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly CatalogContext _writeContext;

    public UnitOfWork(CatalogContext writeContext)
        => _writeContext = writeContext;

    public async Task<bool> CommitAsync()
    {
        return await _writeContext.SaveChangesAsync() > 0;
    }
}
