using Em.Common.Infraestructure.ResourceManagers;
using EM.Common.Core.ResourceManagers;
using EM.Common.Infraestructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace EM.Common.Extensions.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEMCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IMongoClient>(provider =>
        {
            return new MongoClient(configuration.GetConnectionString("mongoDb"));
        });

        services.AddScoped<MessageContext>();
        services.AddScoped<IResourceManager, ResourceManager>();

        return services;
    }
}
