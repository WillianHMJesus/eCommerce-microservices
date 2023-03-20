using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesHandler : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDTO>>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetAllCategoriesHandler(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public async Task<IEnumerable<CategoryDTO>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        return await _categoryRepository.GetAllCategoriesAsync(query.Page, query.PageSize);
    }
}
