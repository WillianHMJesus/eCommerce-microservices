using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.Persistense.Queries;

public sealed class QueryGetAllCategories : IQueryGetAllCategories
{
    private readonly ReadContext _readContext;

    public QueryGetAllCategories(ReadContext readContext)
        => _readContext = readContext;

    public async Task<IEnumerable<CategoryDTO>> GetAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _readContext.Categories.Find(_ => true)
            .Skip((page - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);
    }
}
