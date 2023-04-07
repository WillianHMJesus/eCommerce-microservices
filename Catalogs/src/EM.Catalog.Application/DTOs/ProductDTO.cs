using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Application.DTOs;

public sealed record ProductDTO(Guid Id, string Name, string Description, decimal Value, short Quantity, string Image, bool Available)
{
    public static explicit operator ProductDTO(Product product)
    {
        return new ProductDTO(
            product.Id,
            product.Name,
            product.Description,
            product.Value,
            product.Quantity,
            product.Image,
            product.Available);
    }

    public CategoryDTO Category { get; set; } = default!;
}

