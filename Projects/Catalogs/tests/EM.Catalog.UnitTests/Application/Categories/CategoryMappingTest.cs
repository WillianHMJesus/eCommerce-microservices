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
    [Trait("Test", "Map:CategoryDTOToCategoryAddedEvent")]
    public void Map_CategoryDTOToCategoryAddedEvent_ShouldReturnValidCategoryAddedEvent(
        CategoryMapping sut,
        CategoryDTO category)
    {
        //Arrange
        ITypeMapper<CategoryDTO, CategoryAddedEvent> mapper = sut;

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
    [Trait("Test", "Map:CategoryDTOToCategoryUpdatedEvent")]
    public void Map_CategoryDTOToCategoryUpdatedEvent_ShouldReturnValidCategoryUpdatedEvent(
        CategoryMapping sut,
        CategoryDTO category)
    {
        //Arrange
        ITypeMapper<CategoryDTO, CategoryUpdatedEvent> mapper = sut;

        //Act
        var command = mapper.Map(category);

        //Assert
        command.Should().BeOfType<CategoryUpdatedEvent>();
        command.Id.Should().Be(category.Id);
        command.Code.Should().Be(category.Code);
        command.Name.Should().Be(category.Name);
        command.Description.Should().Be(category.Description);
    }
}
