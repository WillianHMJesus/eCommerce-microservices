using EM.Catalog.Application.Products.Commands.ReactivateProduct;
using EM.Catalog.Domain;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.ReactivateProduct;

public sealed class ReactivateProductCommandValidatorTest
{
    [Theory, AutoProductData]
    [Trait("Test", "Constructor:ValidReactivateProductCommand")]
    public async Task Constructor_ValidReactivateProductCommand_ShouldReturnValidResult(
        ReactivateProductCommandValidator sut,
        ReactivateProductCommand command)
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
        ReactivateProductCommandValidator sut)
    {
        //Arrange
        var command = new ReactivateProductCommand(default);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.InvalidProductId);
    }
}
