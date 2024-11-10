using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Queries.SearchProducts;

public sealed class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, IEnumerable<ProductDTO>>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public SearchProductsQueryHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products =
            await _readRepository.SearchProductsAsync(request.Text, request.Page, request.PageSize, cancellationToken);

        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }
}
