using Elastic.Clients.Elasticsearch;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Domain;
using EM.Catalog.Infraestructure.Persistense.Read;
using EM.Catalog.Infraestructure.Persistense.Write;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WH.Extensions.Microsoft.DependencyInjection;
using WH.SharedKernel;
using WH.SharedKernel.Mediator;
using WH.SimpleMapper.Extensions.Microsoft.DependencyInjection;

namespace EM.Catalog.API.Extensions;

public static class ServiceCollectionExtensions
{
    private const string applicationFullName = "EM.Catalog.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        //Application
        var applicationAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == applicationFullName);
        ArgumentNullException.ThrowIfNull(applicationAssembly);

        services.AddMediator(cfg =>
        {
            cfg.AddPipelineBehavior(typeof(ValidationBehavior<,>));
            cfg.RegisterServicesFromAssemblies(applicationAssembly);
        });

        services.AddValidatorsFromAssembly(applicationAssembly);
        services.AddSimpleMapper(applicationAssembly);

        services.AddDbContext<CatalogContext>(options => options.UseSqlServer(configuration.GetConnectionString("catalog")));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductReadRepository, ProductReadRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(provider =>
        {
            return new ElasticsearchClient(new Uri(configuration.GetValue<string>("ElasticSearch.ConnectionString")!));
        });

        services.AddAuthenticationJwt(configuration["Jwt:SecretKey"] ?? "");

        return services;
    }
}
