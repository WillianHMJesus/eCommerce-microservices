using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.DTOs;

namespace EM.Catalog.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsHandler : IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>
{
    private readonly IQueryGetAllProducts _queryGetAllProducts;

    public GetAllProductsHandler(IQueryGetAllProducts queryGetAllProducts)
        => _queryGetAllProducts = queryGetAllProducts;

    public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        return await _queryGetAllProducts.GetAsync(query.Page, query.PageSize);
    }
}
