using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;

namespace EM.Catalog.Infraestructure.EventsReadDatabase.CategoryAdded;

public sealed class CategoryAddedHandler : IEventHandler<CategoryAddedEvent>
{
    private readonly ReadContext _readContext;

    public CategoryAddedHandler(ReadContext readContext)
        => _readContext = readContext;

    public async Task Handle(CategoryAddedEvent _event, CancellationToken cancellationToken)
    {
        CategoryDTO categoryDTO = (CategoryDTO)_event;
        await _readContext.Categories.InsertOneAsync(categoryDTO, null, cancellationToken);
    }
}
