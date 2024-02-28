using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;

namespace EM.Catalog.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
{
    private readonly IReadRepository _readRepository;

    public GetAllProductsHandler(IReadRepository readRepository)
        => _readRepository = readRepository;

    public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        return await _readRepository.GetAllProductsAsync(query.Page, query.PageSize, cancellationToken);
    }
}
