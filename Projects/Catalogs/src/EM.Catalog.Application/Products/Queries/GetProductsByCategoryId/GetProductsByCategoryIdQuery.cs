using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;

namespace EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;

public sealed record GetProductsByCategoryIdQuery(Guid CategoryId, short Page, short PageSize) 
    : IQuery<IEnumerable<ProductDTO>>
{ }
