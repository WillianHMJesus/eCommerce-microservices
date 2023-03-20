using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Events.CategoryUpdated;

public sealed class CategoryUpdatedHandler : IEventHandler<CategoryUpdatedEvent>
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryUpdatedHandler(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public async Task Handle(CategoryUpdatedEvent _event, CancellationToken cancellationToken)
    {
        CategoryDTO categoryDTO = (CategoryDTO)_event;
        await _categoryRepository.UpdateCategoryAsync(categoryDTO);
    }
}
