using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        return await _productRepository.GetAllProductsAsync(query.Page, query.PageSize);
    }
}
