using AutoMapper;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Application.Categories;

public sealed class CategoryMapping : Profile
{
    public CategoryMapping()
    {
        CreateMap<AddCategoryRequest, AddCategoryCommand>();
        CreateMap<UpdateCategoryRequest, UpdateCategoryCommand>();
        CreateMap<Category, CategoryDTO>();
        CreateMap<Category, CategoryAddedEvent>();
        CreateMap<CategoryAddedEvent, CategoryDTO>();
        CreateMap<Category, CategoryUpdatedEvent>();
        CreateMap<CategoryUpdatedEvent, CategoryDTO>();
        CreateMap<AddCategoryCommand, Category>();
        CreateMap<UpdateCategoryCommand, Category>();
    }
}
