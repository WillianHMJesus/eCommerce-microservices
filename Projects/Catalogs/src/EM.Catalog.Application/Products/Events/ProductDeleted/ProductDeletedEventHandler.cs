using EM.Catalog.Application.Interfaces.Events;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Events.ProductDeleted;

public sealed class ProductDeletedEventHandler : IEventHandler<ProductDeletedEvent>
{
    private readonly IReadRepository _readRepository;

    public ProductDeletedEventHandler(IReadRepository readRepository)
    {
        _readRepository = readRepository;
    }

    public async Task Handle(ProductDeletedEvent _event, CancellationToken cancellationToken)
    {
        await _readRepository.DeleteProductAsync(_event.Id, cancellationToken);
    }
}
