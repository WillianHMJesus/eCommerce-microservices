using AutoFixture.Xunit2;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandValidatorTest
{
    [Theory, AutoProductData]
    [Trait("Test", "Constructor:ValidUpdateProductCommand")]
    public async Task Constructor_ValidUpdateProductCommand_ShouldReturnValidResult(
        [Frozen] Mock<IProductRepository> repositoryMock,
        UpdateProductCommandValidator sut,
        UpdateProductCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Product>());
            
        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoProductData]
    [Trait("Test", "Constructor:FieldsWithDefaultValues")]
    public async Task Constructor_FieldsWithDefaultValues_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        //Arrange
        var command = new UpdateProductCommand(default, default, default, default, default, default, default);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.InvalidProductId);
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.NameNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.DescriptionNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.ValueLessThanEqualToZero);
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.QuantityAddedLessThanOrEqualToZero);
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.ImageNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.InvalidCategoryId);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Constructor:FieldsWithNullValues")]
    public async Task Constructor_FieldsWithNullValues_ShouldReturnInvalidResult(
        UpdateProductCommandValidator sut)
    {
        //Arrange
        var command = new UpdateProductCommand(default, null, null, default, default, null, default);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.NameNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.DescriptionNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.ImageNullOrEmpty);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Constructor:ProductNotFound")]
    public async Task Constructor_ProductNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IProductRepository> repositoryMock,
        UpdateProductCommandValidator sut,
        UpdateProductCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Product);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.ProductNotFound);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Constructor:ProductHasAlreadyBeenRegistered")]
    public async Task Constructor_ProductHasAlreadyBeenRegistered_ShouldReturnInvalidResult(
        [Frozen] Mock<IProductRepository> repositoryMock,
        UpdateProductCommandValidator sut,
        UpdateProductCommand command,
        Product product)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetByNameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([product]);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.ProductHasAlreadyBeenRegistered);
    }

    [Theory, AutoProductData]
    [Trait("Test", "Constructor:CategoryNotFound")]
    public async Task Constructor_CategoryNotFound_ShouldReturnInvalidResult(
        [Frozen] Mock<IProductRepository> repositoryMock,
        UpdateProductCommandValidator sut,
        UpdateProductCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Category);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.CategoryNotFound);
    }
}
