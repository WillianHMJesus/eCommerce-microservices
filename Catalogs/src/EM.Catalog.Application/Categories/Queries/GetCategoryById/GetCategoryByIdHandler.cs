using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDTO?>
{
    private readonly ICategoryRepository _categoryRepository;

    public GetCategoryByIdHandler(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public async Task<CategoryDTO?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        Category? category = await _categoryRepository.GetCategoryByIdAsync(query.Id);

        if (category == null)
            return null;

        return (CategoryDTO)category;
    }
}
