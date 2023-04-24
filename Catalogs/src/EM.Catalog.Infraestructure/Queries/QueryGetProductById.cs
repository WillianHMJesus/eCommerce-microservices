using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;

namespace EM.Catalog.Infraestructure.Queries;

public sealed class QueryGetProductById : IQueryGetProductById
{
    private readonly IDatabaseReadManager _databaseManager;

    public QueryGetProductById(IDatabaseReadManager databaseManager)
        => _databaseManager = databaseManager;

    public async Task<ProductDTO?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _databaseManager.GetProductByIdAsync(id, cancellationToken);
    }
}
