namespace EM.Catalog.Application.Categories.Models;

public sealed record UpdateCategoryRequest
{
    public Guid Id { get; set; }
    public short Code { get; set; }
    public string Name { get; set; } = ""!;
    public string Description { get; set; } = ""!;
}
