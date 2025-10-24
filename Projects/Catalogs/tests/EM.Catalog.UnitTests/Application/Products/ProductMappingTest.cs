using EM.Catalog.Application.Products;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using WH.SimpleMapper;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products;

public sealed class ProductMappingTest
{
    [Theory, AutoProductData]
    [Trait("Test", "Map:AddProductCommandToProduct")]
    public void Map_AddProductCommandToProduct_ShouldReturnValidProduct(
    ProductMapping sut,
    AddProductCommand command)
    {
        //Arrange & Act
        var product = sut.Map(command);

        //Assert
        product.Should().BeOfType<Product>();
        product.Name.Should().Be(command.Name);
        product.Description.Should().Be(command.Description);
        product.Value.Should().Be(command.Value);
        product.Image.Should().Be(command.Image);
        product.CategoryId.Should().Be(command.CategoryId);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Map:UpdateProductCommandToProduct")]
    public void Map_UpdateProductCommandToProduct_ShouldReturnValidProduct(
        ProductMapping sut,
        UpdateProductCommand command)
    {
        //Arrange & Act
        var product = sut.Map(command);

        //Assert
        product.Should().BeOfType<Product>();
        product.Id.Should().Be(command.Id);
        product.Name.Should().Be(command.Name);
        product.Description.Should().Be(command.Description);
        product.Value.Should().Be(command.Value);
        product.Image.Should().Be(command.Image);
        product.CategoryId.Should().Be(command.CategoryId);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Map:ProductDTOToProductAddedEvent")]
    public void Map_ProductDTOToProductAddedEvent_ShouldReturnValidProductAddedEvent(
        ProductMapping sut,
        ProductDTO product)
    {
        //Arrange
        ITypeMapper<ProductDTO, ProductAddedEvent> mapper = sut;

        //Arrange & Act
        var command = mapper.Map(product);

        //Assert
        command.Should().BeOfType<ProductAddedEvent>();
        command.Id.Should().Be(product.Id);
        command.Name.Should().Be(product.Name);
        command.Description.Should().Be(product.Description);
        command.Value.Should().Be(product.Value);
        command.Image.Should().Be(product.Image);
        command.Quantity.Should().Be(product.Quantity);
        command.Available.Should().Be(product.Available);
        command.Category.Should().Be(product.Category);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Map:ProductDTOToProductUpdatedEvent")]
    public void Map_ProductDTOToProductUpdatedEvent_ShouldReturnValidProductUpdatedEvent(
        ProductMapping sut,
        ProductDTO product)
    {
        //Arrange
        ITypeMapper<ProductDTO, ProductUpdatedEvent> mapper = sut;

        //Act
        var command = mapper.Map(product);

        //Assert
        command.Should().BeOfType<ProductUpdatedEvent>();
        command.Id.Should().Be(product.Id);
        command.Name.Should().Be(product.Name);
        command.Description.Should().Be(product.Description);
        command.Value.Should().Be(product.Value);
        command.Image.Should().Be(product.Image);
        command.Quantity.Should().Be(product.Quantity);
        command.Available.Should().Be(product.Available);
        command.Category.Should().Be(product.Category);
    }
}
