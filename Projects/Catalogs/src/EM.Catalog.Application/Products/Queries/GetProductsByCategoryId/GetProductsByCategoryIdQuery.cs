using WH.SharedKernel.Abstractions;

namespace EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;

public sealed record GetProductsByCategoryIdQuery(Guid CategoryId, short Page, short PageSize) 
    : IQuery<IEnumerable<ProductDTO>>
{ }
