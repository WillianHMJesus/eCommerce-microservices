using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;

namespace EM.Catalog.Infraestructure.Queries;

public sealed class QueryGetAllProducts : IQueryGetAllProducts
{
    private readonly IDatabaseReadManager _databaseManager;

    public QueryGetAllProducts(IDatabaseReadManager databaseManager)
        => _databaseManager = databaseManager;

    public async Task<IEnumerable<ProductDTO>> GetAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _databaseManager.GetAllProductsAsync(page, pageSize, cancellationToken);
    }
}
