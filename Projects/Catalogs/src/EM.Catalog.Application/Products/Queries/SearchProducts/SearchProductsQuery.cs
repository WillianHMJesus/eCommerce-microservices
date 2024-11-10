using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;

namespace EM.Catalog.Application.Products.Queries.SearchProducts;

public sealed record SearchProductsQuery(string Text, short Page, short PageSize)
    : IQuery<IEnumerable<ProductDTO>>
{ }
