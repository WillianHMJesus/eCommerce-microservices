using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Application.Products.Queries.SearchProducts;
using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Products.Queries;

public sealed class ProductQueryHandler(IProductReadRepository repository) :
    IQueryHandler<GetAllProductsQuery, IEnumerable<ProductDTO>>,
    IQueryHandler<GetProductByIdQuery, ProductDTO?>,
    IQueryHandler<GetProductsByCategoryIdQuery, IEnumerable<ProductDTO>>,
    IQueryHandler<SearchProductsQuery, IEnumerable<ProductDTO>>
{
    public async Task<IEnumerable<ProductDTO>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync(query.Page, query.PageSize, cancellationToken);
    }

    public async Task<ProductDTO?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(query.Id, cancellationToken);
    }

    public async Task<IEnumerable<ProductDTO>> Handle(GetProductsByCategoryIdQuery query, CancellationToken cancellationToken)
    {
        return await repository.GetByCategoryIdAsync(query.CategoryId, query.Page, query.PageSize, cancellationToken);
    }

    public async Task<IEnumerable<ProductDTO>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        return await repository.SearchAsync(request.Text, request.Page, request.PageSize, cancellationToken);
    }
}
