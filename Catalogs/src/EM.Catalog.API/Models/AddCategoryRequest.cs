using EM.Catalog.Application.Categories.Commands.AddCategory;

namespace EM.Catalog.API.Models;

public sealed record AddCategoryRequest
{
    public short Code { get; set; }
    public string Name { get; set; } = ""!;
    public string Description { get; set; } = ""!;

    public static explicit operator AddCategoryCommand(AddCategoryRequest addCategoryRequest)
    {
        return new AddCategoryCommand(
            addCategoryRequest.Code,
            addCategoryRequest.Name,
            addCategoryRequest.Description);
    }
}
