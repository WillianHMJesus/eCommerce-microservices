using EM.Catalog.Application.Categories.Queries.GetCategoryById;
using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read.Managers;

namespace EM.Catalog.Infraestructure.Queries;

public sealed class QueryGetCategoryById : IQueryGetCategoryById
{
    private readonly IDatabaseReadManager _databaseManager;

    public QueryGetCategoryById(IDatabaseReadManager databaseManager)
        => _databaseManager = databaseManager;

    public async Task<CategoryDTO?> GetAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _databaseManager.GetCategoryByIdAsync(id, cancellationToken);
    }
}
