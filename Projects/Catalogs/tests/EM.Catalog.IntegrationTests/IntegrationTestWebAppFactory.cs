using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using EM.Catalog.Infraestructure.Persistense.Write;
using Testcontainers.MongoDb;
using MongoDB.Driver;

namespace EM.Catalog.IntegrationTests;

public class IntegrationTestWebAppFactory
    : WebApplicationFactory<Program>,
    IAsyncLifetime
{
    private readonly MsSqlContainer _writeDbContainer = new MsSqlBuilder()
        .WithImage("ecommerce-microservices-mssql:latest")
        .WithPassword("Passw0rd")
        .Build();

    private readonly MongoDbContainer _readDbContainer = new MongoDbBuilder()
        .WithImage("ecommerce-microservices-mongo:latest")
        .WithUsername("root")
        .WithPassword("Passw0rd")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            ConfigMsSqlContainer(services);
            ConfigMongoDbContainer(services);
        });
    }

    public async Task InitializeAsync()
    {
        await _writeDbContainer.StartAsync();
        await _readDbContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _writeDbContainer.StopAsync();
        await _readDbContainer.StopAsync();
    }

    private void ConfigMsSqlContainer(IServiceCollection services)
    {
        var descriptorType = typeof(DbContextOptions<CatalogContext>);
        var descriptor = services.SingleOrDefault(s => s.ServiceType == descriptorType);

        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }

        string connectionString = _writeDbContainer.GetConnectionString().Replace("Database=master", "Database=Catalog");
        services.AddDbContext<CatalogContext>(options => options.UseSqlServer(connectionString));
    }

    private void ConfigMongoDbContainer(IServiceCollection services)
    {
        var descriptorType = typeof(IMongoClient);
        var descriptors = services.Where(s => s.ServiceType == descriptorType).ToList();

        descriptors.ForEach(x => services.Remove(x));

        services.AddScoped<IMongoClient>(service =>
        {
            return new MongoClient(_readDbContainer.GetConnectionString());
        });
    }
}
