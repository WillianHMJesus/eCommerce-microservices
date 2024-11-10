using AutoMapper;
using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, CategoryDTO?>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<CategoryDTO?> Handle(GetCategoryByIdQuery query, CancellationToken cancellationToken)
    {
        Category? category = 
            await _readRepository.GetCategoryByIdAsync(query.Id, cancellationToken);
        
        return _mapper.Map<CategoryDTO?>(category);
    }
}
