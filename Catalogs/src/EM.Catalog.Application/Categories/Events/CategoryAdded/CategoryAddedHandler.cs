using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Events.CategoryAdded;

public class CategoryAddedHandler : IEventHandler<CategoryAddedEvent>
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryAddedHandler(ICategoryRepository categoryRepository)
        => _categoryRepository = categoryRepository;

    public async Task Handle(CategoryAddedEvent _event, CancellationToken cancellationToken)
    {
        CategoryDTO categoryDTO = (CategoryDTO)_event;
        await _categoryRepository.AddCategoryAsync(categoryDTO);
    }
}
