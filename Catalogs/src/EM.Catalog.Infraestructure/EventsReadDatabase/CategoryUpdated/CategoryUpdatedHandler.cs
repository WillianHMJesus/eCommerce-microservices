using EM.Catalog.Domain.DTOs;
using EM.Catalog.Infraestructure.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Read;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.EventsReadDatabase.CategoryUpdated;

public sealed class CategoryUpdatedHandler : IEventHandler<CategoryUpdatedEvent>
{
    private readonly ReadContext _readContext;

    public CategoryUpdatedHandler(ReadContext readContext)
        => _readContext = readContext;

    public async Task Handle(CategoryUpdatedEvent _event, CancellationToken cancellationToken)
    {
        CategoryDTO categoryDTO = (CategoryDTO)_event;
        await _readContext.Categories.ReplaceOneAsync(x => x.Id == categoryDTO.Id, categoryDTO);
    }
}
