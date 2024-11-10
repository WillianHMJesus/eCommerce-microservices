using WriteCatalogContext = EM.Catalog.Infraestructure.Persistense.Write.CatalogContext;
using ReadCatalogContext = EM.Catalog.Infraestructure.Persistense.Read.CatalogContext;
using EM.Catalog.Application;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Read;
using EM.Catalog.Infraestructure.Persistense.Write;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using EM.Catalog.Application.Categories.Validations;
using EM.Catalog.Application.Products.Validations;

namespace EM.Catalog.API;

public static class Extension
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AssemblyReference.Assembly));
        services.AddAutoMapper(AssemblyReference.Assembly);
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddScoped<ICategoryValidations,  CategoryValidations>();
        services.AddScoped<IProductValidations, ProductValidations>();

        services.AddDbContext<WriteCatalogContext>(options => options.UseSqlServer(configuration.GetConnectionString("catalog")));
        services.AddScoped<IMongoClient>(provider =>
        {
            return new MongoClient(configuration.GetConnectionString("mongoDb"));
        });

        services.AddScoped<WriteCatalogContext>();
        services.AddScoped<ReadCatalogContext>();
        services.AddScoped<IWriteRepository, WriteRepository>();
        services.AddScoped<IReadRepository, ReadRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
