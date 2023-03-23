using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Queries;

public class QueryGetProductById : IQueryGetProductById
{
    private readonly ReadContext _readContext;

    public QueryGetProductById(ReadContext readContext)
        => _readContext = readContext;

    public async Task<ProductDTO> GetAsync(Guid id)
    {
        return await _readContext.Products.Find(x => x.Id == id)
            .FirstOrDefaultAsync();
    }
}
