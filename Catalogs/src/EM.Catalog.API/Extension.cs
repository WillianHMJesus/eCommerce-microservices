using EM.Catalog.Application;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Decorators;
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
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.Configure<CatalogDatabaseSettings>(configuration.GetSection("CatalogDatabase"));
        services.AddDbContext<WriteContext>(options => options.UseSqlServer(configuration.GetConnectionString("Catalog")));

        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.Decorate<ICategoryRepository, CategoryRepositoryDecorator>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.Decorate<IProductRepository, ProductRepositoryDecorator>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<WriteContext>();
        services.AddScoped<ReadContext>();

        return services;
    }
}
