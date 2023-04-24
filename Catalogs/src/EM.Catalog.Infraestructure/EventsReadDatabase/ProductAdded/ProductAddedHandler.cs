using EM.Catalog.Application.DTOs;
using EM.Catalog.Infraestructure.Persistense.Read.Managers;

namespace EM.Catalog.Infraestructure.EventsReadDatabase.ProductAdded;

public sealed class ProductAddedHandler : IEventHandler<ProductAddedEvent>
{
    private readonly IDatabaseReadManager _databaseManager;

    public ProductAddedHandler(IDatabaseReadManager databaseManager)
        => _databaseManager = databaseManager;

    public async Task Handle(ProductAddedEvent _event, CancellationToken cancellationToken)
    {
        ProductDTO productDTO = (ProductDTO)_event;
        await _databaseManager.AddProductAsync(productDTO, cancellationToken);
    }
}
