using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public GetAllProductsQueryHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products = 
            await _readRepository.GetAllProductsAsync(query.Page, query.PageSize, cancellationToken);

        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }
}
