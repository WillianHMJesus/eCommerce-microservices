using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;

namespace EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;

public sealed class GetProductsByCategoryIdHandler : IQueryHandler<GetProductsByCategoryIdQuery, IEnumerable<ProductDTO>>
{
    private readonly IReadRepository _readRepository;

    public GetProductsByCategoryIdHandler(IReadRepository readRepository)
        => _readRepository = readRepository;

    public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByCategoryIdQuery query, CancellationToken cancellationToken)
    {
        return await _readRepository.GetProductsByCategoryIdAsync(query.CategoryId, cancellationToken);
    }
}
