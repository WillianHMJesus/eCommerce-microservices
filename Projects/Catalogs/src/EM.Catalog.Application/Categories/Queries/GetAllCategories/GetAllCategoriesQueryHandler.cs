using AutoMapper;
using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, IEnumerable<CategoryDTO>>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public GetAllCategoriesQueryHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CategoryDTO>> Handle(GetAllCategoriesQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Category> categories = await _readRepository.GetAllCategoriesAsync(query.Page, query.PageSize, cancellationToken);
        
        return _mapper.Map<IEnumerable<CategoryDTO>>(categories);
    }
}
