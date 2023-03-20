using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductDTO?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<ProductDTO?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetProductByIdAsync(query.Id);

        if (product == null)
            return null;

        return (ProductDTO)product;
    }
}
