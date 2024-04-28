using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductDTO?>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public GetProductByIdQueryHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<ProductDTO?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        Product? product = await _readRepository.GetProductByIdAsync(query.Id, cancellationToken);

        return _mapper.Map<ProductDTO?>(product);
    }
}
