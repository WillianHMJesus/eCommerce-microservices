using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read;

namespace EM.Catalog.Infraestructure.EventsReadDatabase.ProductUpdated;

public class ProductUpdatedHandler : IEventHandler<ProductUpdatedEvent>
{
    private readonly IDatabaseReadManager _databaseManager;

    public ProductUpdatedHandler(IDatabaseReadManager databaseManager)
        => _databaseManager = databaseManager;

    public async Task Handle(ProductUpdatedEvent _event, CancellationToken cancellationToken)
    {
        ProductDTO productDTO = (ProductDTO)_event;
        await _databaseManager.UpdateProductAsync(productDTO, cancellationToken);
    }
}
