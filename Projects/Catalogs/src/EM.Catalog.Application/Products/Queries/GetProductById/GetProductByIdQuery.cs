using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Products.Queries.GetProductById;

public sealed record GetProductByIdQuery(Guid Id) 
    : IQuery<ProductDTO?>
{ }
