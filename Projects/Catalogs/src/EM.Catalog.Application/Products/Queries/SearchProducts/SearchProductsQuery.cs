using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Products.Queries.SearchProducts;

public sealed record SearchProductsQuery(string Text, short Page, short PageSize)
    : IQuery<IEnumerable<ProductDTO>>
{ }
