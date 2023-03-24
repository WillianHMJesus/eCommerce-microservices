using EM.Catalog.Application;
using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Application.Categories.Queries.GetCategoryById;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Queries;
using EM.Catalog.Infraestructure.Persistense.Read;
using EM.Catalog.Infraestructure.Persistense.Repositories;
using EM.Catalog.Infraestructure.Persistense.Write;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EM.Catalog.API;

public static class Extension
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(AssemblyReference.Assembly);
        services.AddMediatR(Infraestructure.AssemblyReference.Assembly);
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.Configure<CatalogDatabaseSettings>(configuration.GetSection("CatalogDatabase"));
        services.AddDbContext<WriteContext>(options => options.UseSqlServer(configuration.GetConnectionString("Catalog")));

        services.AddScoped<WriteContext>();
        services.AddScoped<ReadContext>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IProductRepository, ProductRepository>();
        services.Decorate<IProductRepository, ProductRepositoryDecorator>();

        services.AddScoped<IQueryGetAllCategories, QueryGetAllCategories>();
        services.AddScoped<IQueryGetCategoryById, QueryGetCategoryById>();
        services.AddScoped<IQueryGetAllProducts, QueryGetAllProducts>();
        services.AddScoped<IQueryGetProductById, QueryGetProductById>();
        services.AddScoped<IQueryGetProductsByCategoryId, QueryGetProductsByCategoryId>();

        return services;
    }
}
