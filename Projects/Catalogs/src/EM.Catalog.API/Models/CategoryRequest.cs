namespace EM.Catalog.API.Models;

public sealed record CategoryRequest
{
    public short Code { get; set; }
    public string Name { get; set; } = ""!;
    public string Description { get; set; } = ""!;
}
