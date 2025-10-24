using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Application.Categories.Queries.GetCategoryById;
using EM.Catalog.Application.Interfaces;
using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Categories.Queries;

public sealed class CategoryQueryHandler(IProductReadRepository repository) :
    IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDTO>>,
    IQueryHandler<GetCategoryByIdQuery, CategoryDTO?>
{
    public async Task<IEnumerable<CategoryDTO>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetAllCategoriesAsync(query.Page, query.PageSize, cancellationToken);
    }

    public async Task<CategoryDTO?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetCategoryByIdAsync(query.Id, cancellationToken);
    }
}
