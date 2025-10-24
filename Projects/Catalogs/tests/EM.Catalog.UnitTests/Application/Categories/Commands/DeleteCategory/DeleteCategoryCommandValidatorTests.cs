using EM.Catalog.Domain.Entities;
using EM.Catalog.UnitTests.CustomAutoData;
using Xunit;
using EM.Catalog.Application.Categories.Commands.DeleteCategory;
using FluentValidation.Results;
using FluentAssertions;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandValidatorTests
{
    [Theory, AutoCategoryData]
    [Trait("Test", "Constructor:ValidDeleteCategoryCommand")]
    public async Task Constructor_ValidDeleteCategoryCommand_ShouldReturnValidResult(
        DeleteCategoryCommandValidator sut,
        DeleteCategoryCommand command)
    {
        //Arrange & Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Constructor:DefaultCommandId")]
    public async Task Constructor_DefaultCommandId_ShouldReturnInvalidResult(
        DeleteCategoryCommandValidator sut)
    {
        //Arrange
        var command = new DeleteCategoryCommand(default);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.CategoryInvalidId);
    }
}
