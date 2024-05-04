using AutoMapper;
using EM.Catalog.Application.Interfaces.Events;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Events.CategoryAdded;

public sealed class CategoryAddedEventHandler : IEventHandler<CategoryAddedEvent>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public CategoryAddedEventHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task Handle(CategoryAddedEvent _event, CancellationToken cancellationToken)
    {
        Category category = _mapper.Map<Category>(_event);
        await _readRepository.AddCategoryAsync(category, cancellationToken);
    }
}
