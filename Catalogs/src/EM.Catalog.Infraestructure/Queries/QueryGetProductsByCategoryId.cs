using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Queries;

public sealed class QueryGetProductsByCategoryId : IQueryGetProductsByCategoryId
{
    private readonly ReadContext _readContext;

    public QueryGetProductsByCategoryId(ReadContext readContext)
        => _readContext = readContext;

    public async Task<IEnumerable<ProductDTO>> GetAsync(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _readContext.Products.Find(x => x.Category.Id == categoryId)
            .ToListAsync(cancellationToken);
    }
}
