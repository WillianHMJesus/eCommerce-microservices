using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Interfaces.Events;
using EM.Catalog.Application.Products.Models;

namespace EM.Catalog.Application.Products.Events.ProductUpdated;

public class ProductUpdatedHandler : IEventHandler<ProductUpdatedEvent>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public ProductUpdatedHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task Handle(ProductUpdatedEvent _event, CancellationToken cancellationToken)
    {
        ProductDTO productDTO = _mapper.Map<ProductDTO>(_event);
        await _readRepository.UpdateProductAsync(productDTO, cancellationToken);
    }
}
