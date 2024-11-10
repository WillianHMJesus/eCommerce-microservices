using AutoMapper;
using EM.Catalog.Application.Categories.Models;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Application.Products.Models;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Application.Products;

public sealed class ProductMapping : Profile
{
    public ProductMapping()
    {
        CreateMap<ProductRequest, AddProductCommand>();
        CreateMap<Product, ProductAddedEvent>().ReverseMap();
        CreateMap<Product, ProductUpdatedEvent>().ReverseMap();
        CreateMap<Product?, ProductDTO?>();
        CreateMap<Category?, CategoryDTO?>();

        CreateMap<(Guid, ProductRequest), UpdateProductCommand>()
            .ForCtorParam("Id", x => x.MapFrom(src => src.Item1))
            .ForCtorParam("Name", x => x.MapFrom(src => src.Item2.Name))
            .ForCtorParam("Description", x => x.MapFrom(src => src.Item2.Description))
            .ForCtorParam("Value", x => x.MapFrom(src => src.Item2.Value))
            .ForCtorParam("Quantity", x => x.MapFrom(src => src.Item2.Quantity))
            .ForCtorParam("Image", x => x.MapFrom(src => src.Item2.Image))
            .ForCtorParam("CategoryId", x => x.MapFrom(src => src.Item2.CategoryId));

        CreateMap<AddProductCommand, Product>()
            .ForCtorParam("name", x => x.MapFrom(src => src.Name))
            .ForCtorParam("description", x => x.MapFrom(src => src.Description))
            .ForCtorParam("value", x => x.MapFrom(src => src.Value))
            .ForCtorParam("image", x => x.MapFrom(src => src.Image))
            .ForCtorParam("categoryId", x => x.MapFrom(src => src.CategoryId))
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Available, opt => opt.Ignore())
            .ForMember(x => x.Category, opt => opt.Ignore())
            .ForMember(x => x.Active, opt => opt.Ignore());

        CreateMap<UpdateProductCommand, Product>()
            .ForCtorParam("name", x => x.MapFrom(src => src.Name))
            .ForCtorParam("description", x => x.MapFrom(src => src.Description))
            .ForCtorParam("value", x => x.MapFrom(src => src.Value))
            .ForCtorParam("image", x => x.MapFrom(src => src.Image))
            .ForCtorParam("categoryId", x => x.MapFrom(src => src.CategoryId))
            .ForMember(x => x.Available, opt => opt.Ignore())
            .ForMember(x => x.Category, opt => opt.Ignore())
            .ForMember(x => x.Active, opt => opt.Ignore());
    }
}
