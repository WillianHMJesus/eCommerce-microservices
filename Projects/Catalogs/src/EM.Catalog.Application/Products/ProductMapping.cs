using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain;
using WH.SimpleMapper;

namespace EM.Catalog.Application.Products;

public sealed class ProductMapping :
    ITypeMapper<AddProductCommand, Product>,
    ITypeMapper<UpdateProductCommand, Product>,
    ITypeMapper<ProductDTO, ProductAddedEvent>,
    ITypeMapper<ProductDTO, ProductUpdatedEvent>
{
    public Product Map(AddProductCommand command)
    {
        return Product.Create(command.Name, command.Description, command.Value, command.Image, command.CategoryId);
    }

    public Product Map(UpdateProductCommand command)
    {
        return Product.Load(command.Id, command.Name, command.Description, command.Value, command.Image, command.CategoryId);
    }

    ProductAddedEvent ITypeMapper<ProductDTO, ProductAddedEvent>.Map(ProductDTO product)
    {
        return new ProductAddedEvent
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Value = product.Value,
            Image = product.Image,
            Quantity = product.Quantity,
            Available = product.Available,
            Category = product.Category
        };
    }

    ProductUpdatedEvent ITypeMapper<ProductDTO, ProductUpdatedEvent>.Map(ProductDTO product)
    {
        return new ProductUpdatedEvent
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Value = product.Value,
            Image = product.Image,
            Quantity = product.Quantity,
            Available = product.Available,
            Category = product.Category
        };
    }
}
