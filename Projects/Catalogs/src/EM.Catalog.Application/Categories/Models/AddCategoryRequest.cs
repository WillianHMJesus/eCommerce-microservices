namespace EM.Catalog.Application.Categories.Models;

public sealed record AddCategoryRequest
{
    public short Code { get; set; }
    public string Name { get; set; } = ""!;
    public string Description { get; set; } = ""!;
}
