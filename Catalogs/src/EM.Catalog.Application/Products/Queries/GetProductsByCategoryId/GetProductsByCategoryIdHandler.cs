using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.DTOs;

namespace EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;

public sealed class GetProductsByCategoryIdHandler : IQueryHandler<GetProductsByCategoryIdQuery, IEnumerable<ProductDTO>>
{
    private readonly IQueryGetProductsByCategoryId _queryGetProductsByCategoryId;

    public GetProductsByCategoryIdHandler(IQueryGetProductsByCategoryId queryGetProductsByCategoryId)
        => _queryGetProductsByCategoryId = queryGetProductsByCategoryId;

    public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByCategoryIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryGetProductsByCategoryId.GetAsync(query.CategoryId);
    }
}
