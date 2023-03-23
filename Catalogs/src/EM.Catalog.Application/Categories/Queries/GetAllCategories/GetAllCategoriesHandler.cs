using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.DTOs;

namespace EM.Catalog.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesHandler : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDTO>>
{
    private readonly IQueryGetAllCategories _queryGetAllCategories;

    public GetAllCategoriesHandler(IQueryGetAllCategories queryGetAllCategories)
        => _queryGetAllCategories = queryGetAllCategories;

    public async Task<IEnumerable<CategoryDTO>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        return await _queryGetAllCategories.GetAsync(query.Page, query.PageSize);
    }
}
