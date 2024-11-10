using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;

namespace EM.Catalog.Application.Products.Queries.GetAllProducts;

public sealed record GetAllProductsQuery(short Page, short PageSize) 
    : IQuery<IEnumerable<ProductDTO>>
{ }
