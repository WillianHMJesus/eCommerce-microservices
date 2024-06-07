using Em.Common.Infraestructure.DatabaseSettings;
using Em.Common.Infraestructure.ResourceManagers;
using EM.Common.Core.ResourceManagers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EM.Common.Extensions.DependencyInjection;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddEMCommon(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MessageDatabaseSettings>(configuration.GetSection("MessageDatabase"));

        services.AddScoped<IResourceManager, ResourceManager>();

        return services;
    }
}
