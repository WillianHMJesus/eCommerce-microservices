using AutoMapper;
using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Interfaces.Events;

namespace EM.Catalog.Application.Categories.Events.CategoryAdded;

public sealed class CategoryAddedHandler : IEventHandler<CategoryAddedEvent>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public CategoryAddedHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task Handle(CategoryAddedEvent _event, CancellationToken cancellationToken)
    {
        CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(_event);
        await _readRepository.AddCategoryAsync(categoryDTO, cancellationToken);
    }
}
