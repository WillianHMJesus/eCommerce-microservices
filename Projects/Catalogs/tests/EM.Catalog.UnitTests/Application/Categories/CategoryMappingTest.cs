using EM.Catalog.Application.Categories;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Domain.Entities;
using EM.Catalog.UnitTests.CustomAutoData;
using FluentAssertions;
using WH.SimpleMapper;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories;

public sealed class CategoryMappingTest
{
    [Theory, AutoCategoryData]
    [Trait("Test", "Map:AddCategoryCommandToCategory")]
    public void Map_AddCategoryCommandToCategory_ShouldReturnValidCategory(
        CategoryMapping sut,
        AddCategoryCommand command)
    {
        //Arrange & Act
        var category = sut.Map(command);

        //Assert
        category.Should().BeOfType<Category>();
        category.Code.Should().Be(command.Code);
        category.Name.Should().Be(command.Name);
        category.Description.Should().Be(command.Description);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Map:UpdateCategoryCommandToCategory")]
    public void Map_UpdateCategoryCommandToCategory_ShouldReturnValidCategory(
        CategoryMapping sut,
        UpdateCategoryCommand command)
    {
        //Arrange & Act
        var category = sut.Map(command);

        //Assert
        category.Should().BeOfType<Category>();
        category.Id.Should().Be(command.Id);
        category.Code.Should().Be(command.Code);
        category.Name.Should().Be(command.Name);
        category.Description.Should().Be(command.Description);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Map:CategoryToCategoryAddedEvent")]
    public void Map_CategoryToCategoryAddedEvent_ShouldReturnValidCategoryAddedEvent(
        CategoryMapping sut,
        Category category)
    {
        //Arrange
        ITypeMapper<Category, CategoryAddedEvent> mapper = sut;

        //Arrange & Act
        var command = mapper.Map(category);

        //Assert
        command.Should().BeOfType<CategoryAddedEvent>();
        command.Id.Should().Be(category.Id);
        command.Code.Should().Be(category.Code);
        command.Name.Should().Be(category.Name);
        command.Description.Should().Be(category.Description);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Map:CategoryToCategoryUpdatedEvent")]
    public void Map_CategoryToCategoryUpdatedEvent_ShouldReturnValidCategoryUpdatedEvent(
        CategoryMapping sut,
        Category category)
    {
        //Arrange
        ITypeMapper<Category, CategoryUpdatedEvent> mapper = sut;

        //Act
        var command = mapper.Map(category);

        //Assert
        command.Should().BeOfType<CategoryUpdatedEvent>();
        command.Id.Should().Be(category.Id);
        command.Code.Should().Be(category.Code);
        command.Name.Should().Be(category.Name);
        command.Description.Should().Be(category.Description);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Map:CategoryAddedEventToCategoryDTO")]
    public void Map_CategoryAddedEventToCategoryDTO_ShouldReturnValidCategoryDTO(
        CategoryMapping sut,
        CategoryAddedEvent _event)
    {
        //Arrange & Act
        var category = sut.Map(_event);

        //Assert
        category.Should().BeOfType<CategoryDTO>();
        category.Id.Should().Be(_event.Id);
        category.Code.Should().Be(_event.Code);
        category.Name.Should().Be(_event.Name);
        category.Description.Should().Be(_event.Description);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "Map:CategoryUpdatedEventToCategoryDTO")]
    public void Map_CategoryUpdatedEventToCategoryDTO_ShouldReturnValidCategoryDTO(
        CategoryMapping sut,
        CategoryUpdatedEvent _event)
    {
        //Arrange & Act
        var category = sut.Map(_event);

        //Assert
        category.Should().BeOfType<CategoryDTO>();
        category.Id.Should().Be(_event.Id);
        category.Code.Should().Be(_event.Code);
        category.Name.Should().Be(_event.Name);
        category.Description.Should().Be(_event.Description);
    }
}
