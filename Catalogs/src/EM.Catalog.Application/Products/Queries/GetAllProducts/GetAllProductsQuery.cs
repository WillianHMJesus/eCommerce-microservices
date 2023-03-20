using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;

namespace EM.Catalog.Application.Products.Queries.GetAllProducts;

public sealed record GetAllProductsQuery(short Page, short PageSize) : IQuery<IEnumerable<ProductDTO>>
{ }
