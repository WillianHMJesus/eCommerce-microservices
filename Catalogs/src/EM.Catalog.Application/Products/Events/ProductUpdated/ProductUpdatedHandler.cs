using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Events.ProductUpdated;

public class ProductUpdatedHandler : IEventHandler<ProductUpdatedEvent>
{
    private readonly IProductRepository _productRepository;

    public ProductUpdatedHandler(IProductRepository productRepository)
        => _productRepository = productRepository;

    public async Task Handle(ProductUpdatedEvent _event, CancellationToken cancellationToken)
    {
        ProductDTO productDTO = (ProductDTO)_event;
        await _productRepository.UpdateProductAsync(productDTO);
    }
}
