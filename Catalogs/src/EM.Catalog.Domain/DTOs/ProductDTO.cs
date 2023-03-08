using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain.DTOs;

public sealed record ProductDTO(Guid Id, string Name, string Description, decimal Value, short Quantity, string Image)
{
    public static explicit operator ProductDTO(Product product)
    {
        return new ProductDTO(
            product.Id,
            product.Name,
            product.Description,
            product.Value,
            product.Quantity,
            product.Image);
    }

    public CategoryDTO Category { get; set; } = default!;
}

