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
        CreateMap<CategoryRequest, AddCategoryCommand>();
        CreateMap<Category, CategoryAddedEvent>().ReverseMap();
        CreateMap<Category, CategoryUpdatedEvent>().ReverseMap();
        CreateMap<Category, CategoryDTO>();

        CreateMap<(Guid, CategoryRequest), UpdateCategoryCommand>()
            .ForCtorParam("Id", x => x.MapFrom(src => src.Item1))
            .ForCtorParam("Code", x => x.MapFrom(src => src.Item2.Code))
            .ForCtorParam("Name", x => x.MapFrom(src => src.Item2.Name))
            .ForCtorParam("Description", x => x.MapFrom(src => src.Item2.Description));

        CreateMap<AddCategoryCommand, Category>()
            .ForCtorParam("code", x => x.MapFrom(src => src.Code))
            .ForCtorParam("name", x => x.MapFrom(src => src.Name))
            .ForCtorParam("description", x => x.MapFrom(src => src.Description))
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Active, opt => opt.Ignore());

        CreateMap<UpdateCategoryCommand, Category>()
            .ForCtorParam("code", x => x.MapFrom(src => src.Code))
            .ForCtorParam("name", x => x.MapFrom(src => src.Name))
            .ForCtorParam("description", x => x.MapFrom(src => src.Description))
            .ForMember(x => x.Active, opt => opt.Ignore()); ;
    }
}
