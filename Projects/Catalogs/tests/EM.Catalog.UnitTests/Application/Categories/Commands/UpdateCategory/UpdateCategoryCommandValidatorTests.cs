using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandValidatorTests
{
    [Theory, AutoCategoryData]
    [Trait("Test", "Constructor:ValidUpdateCategoryCommand")]
    public async Task Constructor_ValidUpdateCategoryCommand_ShouldReturnValidResult(
        [Frozen] Mock<IProductRepository> repositoryMock,
        UpdateCategoryCommandValidator sut,
        UpdateCategoryCommand command)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Category>());

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Constructor:FieldsWithDefaultValues")]
    public async Task Constructor_FieldsWithDefaultValues_ShouldReturnInvalidResult(
        UpdateCategoryCommandValidator sut)
    {
        //Arrange
        var command = new UpdateCategoryCommand(Guid.Empty, 0, default, default);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.CategoryInvalidId);
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.CodeLessThanEqualToZero);
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.NameNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.DescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Constructor:FieldsWithNullValues")]
    public async Task Constructor_FieldsWithNullValues_ShouldReturnInvalidResult(
        UpdateCategoryCommandValidator sut)
    {
        //Arrange
        var command = new UpdateCategoryCommand(Guid.Empty, 0, null, null);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.NameNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.DescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Constructor:CategoryNotFound")]
    public async Task Constructor_CategoryNotFound_ShouldReturnValidResult(
        [Frozen] Mock<IProductRepository> repositoryMock,
        UpdateCategoryCommandValidator sut,
        UpdateCategoryCommand command)
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

    [Theory, AutoCategoryData]
    [Trait("Test", "Constructor:CategoryHasAlreadyRegistered")]
    public async Task Constructor_CategoryHasAlreadyRegistered_ShouldReturnValidResult(
        [Frozen] Mock<IProductRepository> repositoryMock,
        UpdateCategoryCommandValidator sut,
        UpdateCategoryCommand command,
        Category category)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([category]);

        //Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.CategoryHasAlreadyBeenRegistered);
    }
}
