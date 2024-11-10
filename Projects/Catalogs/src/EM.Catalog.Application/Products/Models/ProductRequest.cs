namespace EM.Catalog.Application.Products.Models;

public sealed record ProductRequest
{
    public string Name { get; set; } = ""!;
    public string Description { get; set; } = ""!;
    public decimal Value { get; set; }
    public short Quantity { get; set; }
    public string Image { get; set; } = ""!;
    public Guid CategoryId { get; set; }
}
