using EM.Catalog.Application.Products.Commands.AddProduct;

namespace EM.Catalog.API.Models;

public class AddProductRequest
{
    public string Name { get; set; } = ""!;
    public string Description { get; set; } = ""!;
    public decimal Value { get; set; }
    public short Quantity { get; set; }
    public string Image { get; set; } = ""!;

    public static explicit operator AddProductCommand(AddProductRequest addProductRequest)
    {
        return new AddProductCommand(
            addProductRequest.Name, 
            addProductRequest.Description, 
            addProductRequest.Value, 
            addProductRequest.Quantity, 
            addProductRequest.Image);
    }
}
