using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;

public sealed class GetProductsByCategoryIdHandler : IQueryHandler<GetProductsByCategoryIdQuery, IEnumerable<ProductDTO>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsByCategoryIdHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByCategoryIdQuery query, CancellationToken cancellationToken)
    {
        return await _productRepository.GetProductsByCategoryIdAsync(query.CategoryId);
    }
}
