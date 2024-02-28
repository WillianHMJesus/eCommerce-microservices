using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Interfaces.Events;
using EM.Catalog.Application.Products.Models;

namespace EM.Catalog.Application.Products.Events.ProductAdded;

public sealed class ProductAddedHandler : IEventHandler<ProductAddedEvent>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public ProductAddedHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task Handle(ProductAddedEvent _event, CancellationToken cancellationToken)
    {
        ProductDTO productDTO = _mapper.Map<ProductDTO>(_event);
        await _readRepository.AddProductAsync(productDTO, cancellationToken);
    }
}
