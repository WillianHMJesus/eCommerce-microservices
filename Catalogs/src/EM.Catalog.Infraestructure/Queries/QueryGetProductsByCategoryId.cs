using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;

namespace EM.Catalog.Infraestructure.Queries;

public sealed class QueryGetProductsByCategoryId : IQueryGetProductsByCategoryId
{
    private readonly IDatabaseReadManager _databaseManager;

    public QueryGetProductsByCategoryId(IDatabaseReadManager databaseManager)
        => _databaseManager = databaseManager;

    public async Task<IEnumerable<ProductDTO>> GetAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _databaseManager.GetProductsByCategoryIdAsync(categoryId, cancellationToken);
    }
}
