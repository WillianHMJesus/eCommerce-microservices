using EM.Catalog.Application.Products.Commands.InactivateProduct;
using EM.Catalog.Domain;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.InactivateProduct;

public sealed class InactivateProductCommandValidatorTest
{
    [Theory, AutoProductData]
    [Trait("Test", "Constructor:ValidInactivateProductCommand")]
    public async Task Constructor_ValidInactivateProductCommand_ShouldReturnValidResult(
        InactivateProductCommandValidator sut,
        InactivateProductCommand command)
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
        InactivateProductCommandValidator sut)
    {
        //Arrange
        var command = new InactivateProductCommand(default);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Product.InvalidProductId);
    }
}
