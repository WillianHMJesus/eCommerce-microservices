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
    [Trait("Test", "Map:ProductToProductAddedEvent")]
    public void Map_ProductToProductAddedEvent_ShouldReturnValidProductAddedEvent(
        ProductMapping sut,
        Product product)
    {
        //Arrange
        ITypeMapper<Product, ProductAddedEvent> mapper = sut;

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
        command.Category.Id.Should().Be(product.Category.Id);
        command.Category.Code.Should().Be(product.Category.Code);
        command.Category.Name.Should().Be(product.Category.Name);
        command.Category.Description.Should().Be(product.Category.Description);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Map:ProductToProductUpdatedEvent")]
    public void Map_ProductToProductUpdatedEvent_ShouldReturnValidProductUpdatedEvent(
        ProductMapping sut,
        Product product)
    {
        //Arrange
        ITypeMapper<Product, ProductUpdatedEvent> mapper = sut;

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
        command.Category.Id.Should().Be(product.Category.Id);
        command.Category.Code.Should().Be(product.Category.Code);
        command.Category.Name.Should().Be(product.Category.Name);
        command.Category.Description.Should().Be(product.Category.Description);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Map:ProductAddedEventToProductDTO")]
    public void Map_ProductAddedEventToProductDTO_ShouldReturnValidProductDTO(
        ProductMapping sut,
        ProductAddedEvent _event)
    {
        //Arrange & Act
        var product = sut.Map(_event);

        //Assert
        product.Should().BeOfType<ProductDTO>();
        product.Id.Should().Be(_event.Id);
        product.Name.Should().Be(_event.Name);
        product.Description.Should().Be(_event.Description);
        product.Value.Should().Be(_event.Value);
        product.Quantity.Should().Be(_event.Quantity);
        product.Image.Should().Be(_event.Image);
        product.Available.Should().Be(_event.Available);
        product.Category.Id.Should().Be(_event.Category.Id);
        product.Category.Code.Should().Be(_event.Category.Code);
        product.Category.Name.Should().Be(_event.Category.Name);
        product.Category.Description.Should().Be(_event.Category.Description);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Map:ProductUpdatedEventToProductDTO")]
    public void Map_ProductUpdatedEventToProductDTO_ShouldReturnValidProductDTO(
        ProductMapping sut,
        ProductUpdatedEvent _event)
    {
        //Arrange & Act
        var product = sut.Map(_event);

        //Assert
        product.Should().BeOfType<ProductDTO>();
        product.Id.Should().Be(_event.Id);
        product.Name.Should().Be(_event.Name);
        product.Description.Should().Be(_event.Description);
        product.Value.Should().Be(_event.Value);
        product.Quantity.Should().Be(_event.Quantity);
        product.Image.Should().Be(_event.Image);
        product.Available.Should().Be(_event.Available);
        product.Category.Id.Should().Be(_event.Category.Id);
        product.Category.Code.Should().Be(_event.Category.Code);
        product.Category.Name.Should().Be(_event.Category.Name);
        product.Category.Description.Should().Be(_event.Category.Description);
    }
}
