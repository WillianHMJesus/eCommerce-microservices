using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.AddCategory;

#pragma warning disable CS8625
public sealed class AddCategoryCommandValidatorTests
{
    [Theory, AutoCategoryData]
    [Trait("Test", "Constructor:ValidAddCategoryCommand")]
    public async Task Constructor_ValidAddCategoryCommand_ShouldReturnValidResult(
        [Frozen] Mock<IProductRepository> repositoryMock,
        AddCategoryCommandValidator sut,
        AddCategoryCommand command)
    {
        repositoryMock.Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Enumerable.Empty<Category>());

        //Arrange & Act
        ValidationResult result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Constructor:FieldsWithDefaultValues")]
    public async Task Constructor_FieldsWithDefaultValues_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        //Arrange
        var command = new AddCategoryCommand(0, default, default);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.CodeLessThanEqualToZero);
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.NameNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.DescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Constructor:FieldsWithNullValues")]
    public async Task Constructor_FieldsWithNullValues_ShouldReturnInvalidResult(
        AddCategoryCommandValidator sut)
    {
        //Arrange
        var command = new AddCategoryCommand(0, null, null);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.NameNullOrEmpty);
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.DescriptionNullOrEmpty);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Constructor:CategoryHasAlreadyRegistered")]
    public async Task Constructor_CategoryHasAlreadyRegistered_ShouldReturnInvalidResult(
        [Frozen] Mock<IProductRepository> repositoryMock,
        AddCategoryCommandValidator sut,
        AddCategoryCommand command,
        Category category)
    {
        //Arrange
        repositoryMock.Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([category]);

        //Act
        var result = await sut.ValidateAsync(command);

        //Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().Contain(x => x.ErrorMessage == Category.CategoryHasAlreadyBeenRegistered);
    }
}
#pragma warning restore CS8625