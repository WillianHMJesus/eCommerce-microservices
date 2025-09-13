using EM.Authentication.Infraestructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using Xunit;

namespace EM.Authentication.IntegrationTests;

public sealed class IntegrationTestWebAppFactory 
    : WebApplicationFactory<Program>,
    IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlDbContainer = new MsSqlBuilder()
        .WithImage("ecommerce-microservices-mssql:latest")
        .WithPassword("Passw0rd")
        .Build();

    public IntegrationTestWebAppFactory()
    {
        
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigMsSqlContainer);
    }

    public async Task InitializeAsync()
    {
       await _msSqlDbContainer.StartAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await _msSqlDbContainer.StopAsync();
    }

    private void ConfigMsSqlContainer(IServiceCollection services)
    {
        var descriptorType = typeof(DbContextOptions<AuthenticationContext>);
        var descriptor = services.SingleOrDefault(s => s.ServiceType == descriptorType);

        if (descriptor is not null)
        {
            services.Remove(descriptor);
        }

        string connectionString = _msSqlDbContainer.GetConnectionString().Replace("Database=master", "Database=Authentication");
        services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(connectionString));
    }
}
