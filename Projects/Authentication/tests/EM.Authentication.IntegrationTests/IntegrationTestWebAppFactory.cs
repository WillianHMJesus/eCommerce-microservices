using EM.Authentication.Application;
using EM.Authentication.Infraestructure;
using EM.Authentication.IntegrationTests.MockServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.MsSql;
using WH.SharedKernel.Notifications;
using Xunit;

namespace EM.Authentication.IntegrationTests;

[CollectionDefinition("SharedTestCollection")]
public class SharedTestCollection : ICollectionFixture<IntegrationTestWebAppFactory> { }

public sealed class IntegrationTestWebAppFactory
    : WebApplicationFactory<Program>,
    IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlDbContainer = new MsSqlBuilder()
        .WithImage("ecommerce-microservices-mssql:latest")
        .WithPassword("Passw0rd")
        .Build();
    
    private UserResponse? _userResponse;

    public string? AccessToken =>
        _userResponse?.AccessToken;

    public bool TokenIsValid =>
        _userResponse is not null &&
        _userResponse.TokenExpiration > DateTime.UtcNow;

    public void SetUserResponse(UserResponse userResponse) =>
        _userResponse = userResponse;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(ConfigMsSqlContainer);
    }

    public async Task InitializeAsync()
    {
        await _msSqlDbContainer.StartAsync();

        string connectionString = _msSqlDbContainer.GetConnectionString().Replace("Database=master", "Database=Authentication");
        await WaitUntilDatabaseIsResponsive(connectionString);
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

        var smtpEmailDescriptorType = typeof(ISmtpEmailSender);
        var smtpEmailDescriptor = services.SingleOrDefault(s => s.ServiceType == smtpEmailDescriptorType);

        if (smtpEmailDescriptor is not null)
        {
            services.Remove(smtpEmailDescriptor);
        }

        services.AddScoped<ISmtpEmailSender, SmtpEmailSenderMock>();
    }

    private async Task WaitUntilDatabaseIsResponsive(string connectionString)
    {
        var timeout = TimeSpan.FromSeconds(30);
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
                await Task.Delay(500);
            }
        }

        throw new Exception("SQL Server container timeout.");
    }
}
