using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Products.Events.ProductDeleted;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using WH.SharedKernel.Abstractions;
using WH.SimpleMapper;

namespace EM.Catalog.Application.Products.Events;

public sealed class ProductEventHandler(
    IProductReadRepository repository,
    IMapper mapper) :
    IEventHandler<ProductAddedEvent>,
    IEventHandler<ProductDeletedEvent>,
    IEventHandler<ProductUpdatedEvent>
{
    public async Task Handle(ProductAddedEvent _event, CancellationToken cancellationToken)
    {
        var product = mapper.Map<ProductAddedEvent, ProductDTO>(_event);
        await repository.AddAsync(product, cancellationToken);
    }

    public async Task Handle(ProductDeletedEvent _event, CancellationToken cancellationToken)
    {
        await repository.DeleteAsync(_event.Id, cancellationToken);
    }

    public async Task Handle(ProductUpdatedEvent _event, CancellationToken cancellationToken)
    {
        var product = mapper.Map<ProductUpdatedEvent, ProductDTO>(_event);
        await repository.UpdateAsync(product, cancellationToken);
    }
}
