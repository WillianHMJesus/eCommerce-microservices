using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.DTOs;

namespace EM.Catalog.Application.Products.Queries.GetProductById;

public class GetProductByIdQuery : IQuery<ProductDTO?>
{
    public Guid Id { get; set; }
}
