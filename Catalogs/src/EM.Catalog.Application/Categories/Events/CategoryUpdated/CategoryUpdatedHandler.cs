using AutoMapper;
using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Interfaces.Events;

namespace EM.Catalog.Application.Categories.Events.CategoryUpdated;

public sealed class CategoryUpdatedHandler : IEventHandler<CategoryUpdatedEvent>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public CategoryUpdatedHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    } 

    public async Task Handle(CategoryUpdatedEvent _event, CancellationToken cancellationToken)
    {
        CategoryDTO categoryDTO = _mapper.Map<CategoryDTO>(_event);
        await _readRepository.UpdateCategoryAsync(categoryDTO, cancellationToken);
    }
}
