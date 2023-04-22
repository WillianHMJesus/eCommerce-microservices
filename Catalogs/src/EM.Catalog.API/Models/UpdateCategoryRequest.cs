using EM.Catalog.Application.Categories.Commands.UpdateCategory;

namespace EM.Catalog.API.Models;

public sealed record UpdateCategoryRequest
{
    public Guid Id { get; set; }
    public short Code { get; set; }
    public string Name { get; set; } = ""!;
    public string Description { get; set; } = ""!;

    public static explicit operator UpdateCategoryCommand(UpdateCategoryRequest updateCategoryRequest)
    {
        return new UpdateCategoryCommand(
            updateCategoryRequest.Id,
            updateCategoryRequest.Code,
            updateCategoryRequest.Name,
            updateCategoryRequest.Description);
    }
}
