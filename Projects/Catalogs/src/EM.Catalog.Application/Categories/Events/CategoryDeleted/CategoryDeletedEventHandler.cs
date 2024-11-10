using AutoMapper;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Interfaces.Events;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Events.CategoryDeleted;

public sealed class CategoryDeletedEventHandler : IEventHandler<CategoryDeletedEvent>
{
    private readonly IReadRepository _readRepository;

    public CategoryDeletedEventHandler(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task Handle(CategoryDeletedEvent _event, CancellationToken cancellationToken)
    {
        await _readRepository.DeleteCategoryAsync(_event.Id, cancellationToken);
    }
}
