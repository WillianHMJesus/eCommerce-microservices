using AutoFixture;
using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Validations;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Validations;

public sealed class CategoryValidationsTest
{
    [Theory, AutoCategoryData]
    public async Task ValidateCategoryIdAsync_CategoryFound_ShouldReturnTrue(
        CategoryValidations sut,
        Guid categoryId)
    {
        bool result = await sut.ValidateCategoryIdAsync(categoryId, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Theory, AutoCategoryData]
    public async Task ValidateCategoryIdAsync_CategoryNotFound_ShouldReturnFalse(
        [Frozen] Mock<IWriteRepository> repositoryMock,
        CategoryValidations sut,
        Guid categoryId)
    {
        Category? category = null;
        repositoryMock
            .Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        bool result = await sut.ValidateCategoryIdAsync(categoryId, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Theory, AutoCategoryData]
    public async Task ValidateDuplicityAddAsync_CategoriesEmpty_ShouldReturnTrue(
        [Frozen] Mock<IWriteRepository> repositoryMock,
        CategoryValidations sut,
        AddCategoryCommand command)
    {
        repositoryMock
            .Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>());

        bool result = await sut.ValidateDuplicityAsync(command, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Theory, AutoCategoryData]
    public async Task ValidateDuplicityAddAsync_CategoriesNotEmpty_ShouldReturnFalse(
        CategoryValidations sut,
        AddCategoryCommand command)
    {
        bool result = await sut.ValidateDuplicityAsync(command, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Theory, AutoCategoryData]
    public async Task ValidateDuplicityUpdateAsync_CategoriesEmpty_ShouldReturnTrue(
        [Frozen] Mock<IWriteRepository> repositoryMock,
        CategoryValidations sut,
        UpdateCategoryCommand command)
    {
        repositoryMock
            .Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Category>());

        bool result = await sut.ValidateDuplicityAsync(command, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Theory, AutoCategoryData]
    public async Task ValidateDuplicityUpdateAsync_CategoriesNotEmpty_ShouldReturnFalse(
        CategoryValidations sut,
        UpdateCategoryCommand command)
    {
        bool result = await sut.ValidateDuplicityAsync(command, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Theory, AutoCategoryData]
    public async Task ValidateDuplicityUpdateAsync_CategoriesNotEmptyButNotDuplicity_ShouldReturnTrue(
        [Frozen] Mock<IWriteRepository> repositoryMock,
        CategoryValidations sut,
        Category category)
    {
        IEnumerable<Category> categories = new List<Category>() { category }; 
        repositoryMock
            .Setup(x => x.GetCategoriesByCodeOrName(It.IsAny<short>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        UpdateCategoryCommand command = new Fixture().Build<UpdateCategoryCommand>()
            .With(x => x.Id, category.Id)
            .Create();

        bool result = await sut.ValidateDuplicityAsync(command, CancellationToken.None);

        result.Should().BeTrue();
    }
}
