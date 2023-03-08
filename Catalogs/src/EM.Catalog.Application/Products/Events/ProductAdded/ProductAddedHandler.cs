using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Events.ProductAdded;

public class ProductAddedHandler : IEventHandler<ProductAddedEvent>
{
    private readonly IProductRepository _productRepository;

    public ProductAddedHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task Handle(ProductAddedEvent _event, CancellationToken cancellationToken)
    {
        ProductDTO productDTO = (ProductDTO)_event;
        await _productRepository.AddProductReadDatabaseAsync(productDTO);
    }
}

