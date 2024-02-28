using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;

namespace EM.Catalog.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductDTO?>
{
    private readonly IReadRepository _readRepository;

    public GetProductByIdHandler(IReadRepository readRepository)
        => _readRepository = readRepository;

    public async Task<ProductDTO?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        return await _readRepository.GetProductByIdAsync(query.Id, cancellationToken);
    }
}
