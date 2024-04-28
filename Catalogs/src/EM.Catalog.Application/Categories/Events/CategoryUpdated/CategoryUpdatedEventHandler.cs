using AutoMapper;
using EM.Catalog.Application.Interfaces.Events;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Events.CategoryUpdated;

public sealed class CategoryUpdatedEventHandler : IEventHandler<CategoryUpdatedEvent>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public CategoryUpdatedEventHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    } 

    public async Task Handle(CategoryUpdatedEvent _event, CancellationToken cancellationToken)
    {
        Category category = _mapper.Map<Category>(_event);
        await _readRepository.UpdateCategoryAsync(category, cancellationToken);
    }
}
