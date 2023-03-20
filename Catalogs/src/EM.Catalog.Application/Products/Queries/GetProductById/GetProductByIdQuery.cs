using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;

namespace EM.Catalog.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) : IQuery<ProductDTO?>
{ }
