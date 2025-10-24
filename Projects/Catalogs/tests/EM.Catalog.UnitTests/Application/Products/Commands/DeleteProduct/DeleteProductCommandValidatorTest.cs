using EM.Catalog.UnitTests.CustomAutoData;
using Xunit;
using FluentValidation.Results;
using EM.Catalog.Application.Products.Commands.DeleteProduct;
using FluentAssertions;
using EM.Catalog.Domain;

namespace EM.Catalog.UnitTests.Application.Products.Commands.DeleteProduct;
public sealed class DeleteProductCommandValidatorTest
{
    [Theory, AutoProductData]
    [Trait("Test", "Constructor:ValidDeleteProductCommand")]
    public async Task Constructor_ValidDeleteProductCommand_ShouldReturnValidResult(
        DeleteProductCommandValidator sut,
        DeleteProductCommand command)
    {
        //Arrange & Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoProductData]
    [Trait("Test", "Constructor:DefaultCommandId")]
    public async Task Constructor_DefaultCommandId_ShouldReturnInvalidResult(
        DeleteProductCommandValidator sut)
    {
        //Arrange
        var command = new DeleteProductCommand(default);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.InvalidProductId);
    }
}
