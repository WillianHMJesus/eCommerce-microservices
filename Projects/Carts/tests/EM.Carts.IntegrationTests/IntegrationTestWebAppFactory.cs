using EM.Carts.Application.Interfaces.ExternalServices;
using EM.Carts.IntegrationTests.Mocks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Testcontainers.MongoDb;

namespace EM.Carts.IntegrationTests;

public class IntegrationTestWebAppFactory
    : WebApplicationFactory<Program>,
    IAsyncLifetime
{
    private readonly MongoDbContainer _readDbContainer = new MongoDbBuilder()
        .WithImage("ecommerce-microservices-mongodb:latest")
        .WithUsername("root")
        .WithPassword("Passw0rd")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            ConfigMongoDbContainer(services);
            ConfigCatalogExternalService(services);
        });
    }

    public async Task InitializeAsync()
    {
        await _readDbContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _readDbContainer.StopAsync();
    }

    private void ConfigMongoDbContainer(IServiceCollection services)
    {
        Type descriptorType = typeof(IMongoClient);
        List<ServiceDescriptor> descriptors = services.Where(x => x.ServiceType == descriptorType).ToList();

        descriptors.ForEach(x => services.Remove(x));

        services.AddScoped<IMongoClient>(service =>
        {
            return new MongoClient(_readDbContainer.GetConnectionString());
        });
    }

    private void ConfigCatalogExternalService(IServiceCollection services)
    {
        Type descriptorType = typeof(ICatalogExternalService);
        List<ServiceDescriptor> descriptors = services.Where(x => x.ServiceType == descriptorType).ToList();

        descriptors.ForEach(x => services.Remove(x));

        services.AddScoped<ICatalogExternalService>(service =>
        {
            return new MockCatalogExternalService();
        });
    }
}
