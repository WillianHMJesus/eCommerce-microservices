using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Queries.GetProductById;

public class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductDTO?>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task<ProductDTO?> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        Product? product = await _productRepository.GetProductByIdAsync(request.Id);

        if (product == null)
            return null;

        return (ProductDTO)product;
    }
}
