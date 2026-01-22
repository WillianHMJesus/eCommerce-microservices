using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Domain.Entities;
using WH.SharedKernel.Abstractions;
using WH.SimpleMapper;

namespace EM.Catalog.Application.Categories;

public sealed class CategoryMapping :
    ITypeMapper<AddCategoryCommand, Category>,
    ITypeMapper<UpdateCategoryCommand, Category>,
    ITypeMapper<Category, CategoryAddedEvent>,
    ITypeMapper<Category, CategoryUpdatedEvent>,
    ITypeMapper<CategoryAddedEvent, CategoryDTO>,
    ITypeMapper<CategoryUpdatedEvent, CategoryDTO>
{
    public Category Map(AddCategoryCommand command)
    {
        return Category.Create(command.Code, command.Name, command.Description);
    }

    public Category Map(UpdateCategoryCommand command)
    {
        return Category.Load(command.Id, command.Code, command.Name, command.Description);
    }

    CategoryAddedEvent ITypeMapper<Category, CategoryAddedEvent>.Map(Category category)
    {
        return new CategoryAddedEvent
        {
            Id = category.Id,
            Code = category.Code,
            Name = category.Name,
            Description = category.Description
        };
    }

    CategoryUpdatedEvent ITypeMapper<Category, CategoryUpdatedEvent>.Map(Category category)
    {
        return new CategoryUpdatedEvent
        {
            Id = category.Id,
            Code = category.Code,
            Name = category.Name,
            Description = category.Description
        };
    }

    public CategoryDTO Map(CategoryAddedEvent _event)
    {
        return new CategoryDTO(_event.Id, _event.Code, _event.Name, _event.Description);
    }

    public CategoryDTO Map(CategoryUpdatedEvent _event)
    {
        return new CategoryDTO(_event.Id, _event.Code, _event.Name, _event.Description);
    }
}
