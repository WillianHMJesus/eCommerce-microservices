using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Products.Queries.GetAllProducts;

public sealed record GetAllProductsQuery(short Page, short PageSize) 
    : IQuery<IEnumerable<ProductDTO>>
{ }
