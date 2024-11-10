using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Models;

namespace EM.Catalog.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) 
    : IQuery<ProductDTO?>
{ }
