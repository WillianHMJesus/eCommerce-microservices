using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;
using MongoDB.Driver;

namespace EM.Catalog.Infraestructure.EventsReadDatabase.ProductUpdated;

public class ProductUpdatedHandler : IEventHandler<ProductUpdatedEvent>
{
    private readonly ReadContext _readContext;

    public ProductUpdatedHandler(ReadContext readContext)
        => _readContext = readContext;

    public async Task Handle(ProductUpdatedEvent _event, CancellationToken cancellationToken)
    {
        ProductDTO productDTO = (ProductDTO)_event;
        await _readContext.Products.ReplaceOneAsync(x => x.Id == productDTO.Id, productDTO, cancellationToken: cancellationToken);
    }
}
