using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;

namespace EM.Catalog.Infraestructure.Queries;

public sealed class QueryGetAllCategories : IQueryGetAllCategories
{
    private readonly IDatabaseReadManager _databaseManager;

    public QueryGetAllCategories(IDatabaseReadManager databaseManager)
        => _databaseManager = databaseManager;

    public async Task<IEnumerable<CategoryDTO>> GetAsync(short page, short pageSize, CancellationToken cancellationToken)
    {
        return await _databaseManager.GetAllCategoriesAsync(page, pageSize, cancellationToken);
    }
}
