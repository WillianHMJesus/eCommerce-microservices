using EM.Catalog.Application.Categories;

namespace EM.Catalog.Application.Products;

public sealed record ProductDTO(Guid Id, string Name, string Description, decimal Value, short Quantity, string Image, bool Available)
{
    public CategoryDTO Category { get; set; } = default!;
}
