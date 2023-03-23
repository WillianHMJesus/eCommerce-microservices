using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.DTOs;

namespace EM.Catalog.Application.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDTO?>
{
    private readonly IQueryGetCategoryById _queryGetCategoryById;

    public GetCategoryByIdHandler(IQueryGetCategoryById queryGetCategoryById)
        => _queryGetCategoryById = queryGetCategoryById;

    public async Task<CategoryDTO?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryGetCategoryById.GetAsync(query.Id);
    }
}
