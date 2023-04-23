using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Queries;

public sealed class QueryGetAllProducts : IQueryGetAllProducts
{
    private readonly ReadContext _readContext;

    public QueryGetAllProducts(ReadContext readContext)
        => _readContext = readContext;

    public async Task<IEnumerable<ProductDTO>> GetAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _readContext.Products.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }
}
