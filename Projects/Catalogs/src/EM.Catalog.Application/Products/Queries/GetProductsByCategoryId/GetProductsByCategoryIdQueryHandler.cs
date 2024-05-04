using AutoMapper;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;

namespace EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;

public sealed class GetProductsByCategoryIdQueryHandler : IQueryHandler<GetProductsByCategoryIdQuery, IEnumerable<ProductDTO>>
{
    private readonly IReadRepository _readRepository;
    private readonly IMapper _mapper;

    public GetProductsByCategoryIdQueryHandler(
        IReadRepository readRepository,
        IMapper mapper)
    {
        _readRepository = readRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByCategoryIdQuery query, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products = await _readRepository.GetProductsByCategoryIdAsync(query.CategoryId, cancellationToken);

        return _mapper.Map<IEnumerable<ProductDTO>>(products);
    }
}
