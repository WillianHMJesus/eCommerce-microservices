using EM.Authentication.Application.Providers;
using EM.Authentication.Domain;
using EM.Authentication.Domain.Notifications;
using EM.Authentication.Infraestructure;
using EM.Authentication.Infraestructure.Notifications;
using EM.Authentication.Infraestructure.Providers;
using EM.Authentication.Infraestructure.Repositories;
using EM.Authentication.Infraestructure.SettingsOptions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WH.Extensions.Microsoft.DependencyInjection;
using WH.SharedKernel;
using WH.SharedKernel.Mediator;
using WH.SimpleMapper.Extensions.Microsoft.DependencyInjection;

namespace EM.Authentication.API.Extensions;

public static class ServiceCollectionExtensions
{
    private const string applicationFullName = "EM.Authentication.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

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

        services.AddScoped<IPasswordProvider, PasswordProvider>();
        services.AddScoped<ITokenProvider, TokenProvider>();
        services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));

        //Infraestructure
        services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(configuration.GetConnectionString("Authentication")));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserEmailNotification, UserEmailNotification>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<EmailNotificationOptions>(configuration.GetSection("EmailNotification"));

        //External
        services.AddAuthenticationJwt(configuration["Jwt:SecretKey"] ?? "");
        services.AddAuthorizationBuilder()
            .AddPolicy("AddUser", policy => policy.RequireRole("AddUser"));
        services.AddEmailSender(ServiceLifetime.Scoped);

        return services;
    }
}
