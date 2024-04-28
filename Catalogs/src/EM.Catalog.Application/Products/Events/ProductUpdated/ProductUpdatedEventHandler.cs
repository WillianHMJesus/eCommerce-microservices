using AutoMapper;
using EM.Catalog.Application.Interfaces.Events;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Events.ProductUpdated;

public class ProductUpdatedEventHandler : IEventHandler<ProductUpdatedEvent>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public ProductUpdatedEventHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task Handle(ProductUpdatedEvent _event, CancellationToken cancellationToken)
    {
        Product product = _mapper.Map<Product>(_event);
        await _readRepository.UpdateProductAsync(product, cancellationToken);
    }
}
