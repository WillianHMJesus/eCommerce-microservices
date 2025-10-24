using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Domain.Entities;
using WH.SimpleMapper;

namespace EM.Catalog.Application.Categories;

public sealed class CategoryMapping :
    ITypeMapper<AddCategoryCommand, Category>,
    ITypeMapper<UpdateCategoryCommand, Category>,
    ITypeMapper<CategoryDTO, CategoryAddedEvent>,
    ITypeMapper<CategoryDTO, CategoryUpdatedEvent>
{
    public Category Map(AddCategoryCommand command)
    {
        return Category.Create(command.Code, command.Name, command.Description);
    }

    public Category Map(UpdateCategoryCommand command)
    {
        return Category.Load(command.Id, command.Code, command.Name, command.Description);
    }

    CategoryAddedEvent ITypeMapper<CategoryDTO, CategoryAddedEvent>.Map(CategoryDTO category)
    {
        return new CategoryAddedEvent
        {
            Id = category.Id, 
            Code = category.Code, 
            Name = category.Name,
            Description = category.Description
        };
    }

    CategoryUpdatedEvent ITypeMapper<CategoryDTO, CategoryUpdatedEvent>.Map(CategoryDTO category)
    {
        return new CategoryUpdatedEvent
        {
            Id = category.Id,
            Code = category.Code,
            Name = category.Name,
            Description = category.Description
        };
    }
}
