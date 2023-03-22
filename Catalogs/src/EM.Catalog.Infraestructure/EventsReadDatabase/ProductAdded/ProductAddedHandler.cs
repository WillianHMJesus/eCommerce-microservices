using EM.Catalog.Domain.DTOs;
using EM.Catalog.Infraestructure.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Read;

namespace EM.Catalog.Infraestructure.EventsReadDatabase.ProductAdded;

public class ProductAddedHandler : IEventHandler<ProductAddedEvent>
{
    private readonly ReadContext _readContext;

    public ProductAddedHandler(ReadContext readContext)
        => _readContext = readContext;

    public async Task Handle(ProductAddedEvent _event, CancellationToken cancellationToken)
    {
        ProductDTO productDTO = (ProductDTO)_event;
        await _readContext.Products.InsertOneAsync(productDTO);
    }
}
