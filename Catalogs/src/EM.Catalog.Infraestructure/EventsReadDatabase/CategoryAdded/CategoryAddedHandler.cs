using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;

namespace EM.Catalog.Infraestructure.EventsReadDatabase.CategoryAdded;

public sealed class CategoryAddedHandler : IEventHandler<CategoryAddedEvent>
{
    private readonly IDatabaseReadManager _databaseManager;

    public CategoryAddedHandler(IDatabaseReadManager databaseManager)
        => _databaseManager = databaseManager;

    public async Task Handle(CategoryAddedEvent _event, CancellationToken cancellationToken)
    {
        CategoryDTO categoryDTO = (CategoryDTO)_event;
        await _databaseManager.AddCategoryAsync(categoryDTO, cancellationToken);
    }
}
