using EM.Catalog.Application.Products.Commands.UpdateProduct;

namespace EM.Catalog.API.Models;

public class UpdateProductRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = ""!;
    public string Description { get; set; } = ""!;
    public decimal Value { get; set; }
    public short Quantity { get; set; }
    public string Image { get; set; } = ""!;
    public Guid CategoryId { get; set; }

    public static explicit operator UpdateProductCommand(UpdateProductRequest updateProductRequest)
    {
        return new UpdateProductCommand(
            updateProductRequest.Id,
            updateProductRequest.Name,
            updateProductRequest.Description,
            updateProductRequest.Value,
            updateProductRequest.Quantity,
            updateProductRequest.Image,
            updateProductRequest.CategoryId);
    }
}
