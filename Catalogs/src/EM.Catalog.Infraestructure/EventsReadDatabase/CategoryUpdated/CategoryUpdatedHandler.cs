using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read.Managers;

namespace EM.Catalog.Infraestructure.EventsReadDatabase.CategoryUpdated;

public sealed class CategoryUpdatedHandler : IEventHandler<CategoryUpdatedEvent>
{
    private readonly IDatabaseReadManager _databaseManager;

    public CategoryUpdatedHandler(IDatabaseReadManager databaseManager)
        => _databaseManager = databaseManager;

    public async Task Handle(CategoryUpdatedEvent _event, CancellationToken cancellationToken)
    {
        CategoryDTO categoryDTO = (CategoryDTO)_event;
        await _databaseManager.UpdateCategoryAsync(categoryDTO, cancellationToken);
    }
}
