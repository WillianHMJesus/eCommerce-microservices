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
        CreateMap<AddProductRequest, AddProductCommand>();
        CreateMap<UpdateProductRequest, UpdateProductCommand>();
        CreateMap<Product, ProductAddedEvent>().ReverseMap();
        CreateMap<Product, ProductUpdatedEvent>().ReverseMap();
        CreateMap<Product?, ProductDTO?>();
        CreateMap<Category?, CategoryDTO?>();
        CreateMap<IEnumerable<Product>, IEnumerable<ProductDTO>>();

        CreateMap<AddProductCommand, Product>()
            .ForCtorParam("name", x => x.MapFrom(src => src.Name))
            .ForCtorParam("description", x => x.MapFrom(src => src.Description))
            .ForCtorParam("value", x => x.MapFrom(src => src.Value))
            .ForCtorParam("image", x => x.MapFrom(src => src.Image))
            .ForCtorParam("categoryId", x => x.MapFrom(src => src.CategoryId))
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Available, opt => opt.Ignore())
            .ForMember(x => x.Category, opt => opt.Ignore());

        CreateMap<UpdateProductCommand, Product>()
            .ForCtorParam("name", x => x.MapFrom(src => src.Name))
            .ForCtorParam("description", x => x.MapFrom(src => src.Description))
            .ForCtorParam("value", x => x.MapFrom(src => src.Value))
            .ForCtorParam("image", x => x.MapFrom(src => src.Image))
            .ForCtorParam("categoryId", x => x.MapFrom(src => src.CategoryId))
            .ForMember(x => x.Category, opt => opt.Ignore());
    }
}
