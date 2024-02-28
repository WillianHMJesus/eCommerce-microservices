using AutoMapper;
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
        CreateMap<Product, ProductDTO>();
        CreateMap<Product, ProductAddedEvent>();
        CreateMap<ProductAddedEvent, ProductDTO>();
        CreateMap<Product, ProductUpdatedEvent>();
        CreateMap<ProductUpdatedEvent, ProductDTO>();
        CreateMap<AddProductCommand, Product>();
        CreateMap<UpdateProductCommand, Product>();
    }
}
