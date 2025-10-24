using EM.Catalog.Application.Categories;
using WH.SharedKernel;

namespace EM.Catalog.Application.Products.Events.ProductUpdated;

public sealed class ProductUpdatedEvent
    : DomainEvent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public decimal Value { get; set; }
    public short Quantity { get; set; }
    public string Image { get; set; } = "";
    public bool Available { get; set; }
    public CategoryDTO Category { get; set; } = default!;
}
