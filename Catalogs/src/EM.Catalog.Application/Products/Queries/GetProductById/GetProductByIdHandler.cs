using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.DTOs;

namespace EM.Catalog.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductDTO?>
{
    private readonly IQueryGetProductById _queryGetProductById;

    public GetProductByIdHandler(IQueryGetProductById queryGetProductById)
        => _queryGetProductById = queryGetProductById;

    public async Task<ProductDTO?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        return await _queryGetProductById.GetAsync(query.Id);
    }
}
