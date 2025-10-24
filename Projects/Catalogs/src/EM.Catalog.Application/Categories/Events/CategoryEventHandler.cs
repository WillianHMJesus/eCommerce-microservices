using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Categories.Events.CategoryDeleted;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Application.Interfaces;
using WH.SharedKernel.Abstractions;
using WH.SimpleMapper;

namespace EM.Catalog.Application.Categories.Events;

public sealed class CategoryEventHandler(
    IProductReadRepository repository,
    IMapper mapper) :
    IEventHandler<CategoryAddedEvent>,
    IEventHandler<CategoryDeletedEvent>,
    IEventHandler<CategoryUpdatedEvent>
{
    public async Task Handle(CategoryAddedEvent _event, CancellationToken cancellationToken)
    {
        var category = mapper.Map<CategoryAddedEvent, CategoryDTO>(_event);
        await repository.AddCategoryAsync(category, cancellationToken);
    }

    public async Task Handle(CategoryDeletedEvent _event, CancellationToken cancellationToken)
    {
        await repository.DeleteCategoryAsync(_event.Id, cancellationToken);
    }

    public async Task Handle(CategoryUpdatedEvent _event, CancellationToken cancellationToken)
    {
        var category = mapper.Map<CategoryUpdatedEvent, CategoryDTO>(_event);
        await repository.UpdateCategoryAsync(category, cancellationToken);
    }
}
