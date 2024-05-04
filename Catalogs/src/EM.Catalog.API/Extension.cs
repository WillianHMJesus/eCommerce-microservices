using EM.Catalog.Application;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.Infraestructure.Persistense.Read;
using EM.Catalog.Infraestructure.Persistense.Write;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EM.Catalog.API;

public static class Extension
{
    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AssemblyReference.Assembly));
        services.AddAutoMapper(AssemblyReference.Assembly);
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.Configure<CatalogDatabaseSettings>(configuration.GetSection("CatalogDatabase"));
        services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration.GetConnectionString("Catalog")));

        services.AddScoped<CatalogContext>();
        services.AddScoped<IWriteRepository, WriteRepository>();
        services.AddScoped<IReadRepository, ReadRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
