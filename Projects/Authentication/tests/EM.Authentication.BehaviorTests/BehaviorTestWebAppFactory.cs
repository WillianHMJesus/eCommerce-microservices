using EM.Authentication.BehaviorTests.MockServices;
using EM.Authentication.Infraestructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using WH.SharedKernel.Notifications;

namespace EM.Authentication.BehaviorTests;

public sealed class BehaviorTestWebAppFactory
    : WebApplicationFactory<Program>,
    IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlDbContainer =
        new MsSqlBuilder("ecommerce-microservices-mssql:latest")
            .WithPassword("Passw0rd")
            .WithEnvironment("MSSQL_DATABASE", "Authentication")
            .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigMsSqlContainer);
    }

    public async Task InitializeAsync()
    {
        await _msSqlDbContainer.StartAsync();

        await WaitUntilDatabaseIsResponsive(_msSqlDbContainer.GetConnectionString());
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

        var smtpEmailDescriptorType = typeof(IEmailSender);
        var smtpEmailDescriptor = services.SingleOrDefault(s => s.ServiceType == smtpEmailDescriptorType);

        if (smtpEmailDescriptor is not null)
        {
            services.Remove(smtpEmailDescriptor);
        }

        services.AddScoped<IEmailSender, SmtpEmailSenderMock>();
    }

    private async Task WaitUntilDatabaseIsResponsive(string connectionString)
    {
        var timeout = TimeSpan.FromMinutes(2);
        var start = DateTime.UtcNow;

        while (DateTime.UtcNow - start < timeout)
        {
            try
            {
                using var connection = new SqlConnection(connectionString);
                await connection.OpenAsync();
                return;
            }
            catch
            {
                await Task.Delay(1000);
            }
        }

        throw new Exception("SQL Server container timeout.");
    }
}
