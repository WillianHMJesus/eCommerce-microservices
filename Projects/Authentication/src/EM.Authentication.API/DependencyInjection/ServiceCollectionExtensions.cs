using Microsoft.EntityFrameworkCore;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using WH.SharedKernel;
using EM.Authentication.Domain;
using EM.Authentication.Infraestructure.Repositories;
using EM.Authentication.Infraestructure;
using WH.SharedKernel.Mediator;
using WH.Extensions.Microsoft.DependencyInjection;
using EM.Authentication.Application.Mappers;
using EM.Authentication.Infraestructure.Providers;
using EM.Authentication.Application.Providers;

namespace EM.Authentication.API.DependencyInjection;

public static class ServiceCollectionExtensions
{
    private const string applicationFullName = "EM.Authentication.Application, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null";

    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        var applicationAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == applicationFullName);
        if (applicationAssembly is null) throw new ArgumentNullException();

        services.AddMediator(cfg =>
        {
            cfg.AddPipelineBehavior(typeof(ValidationBehavior<,>));
            cfg.RegisterServicesFromAssemblies(applicationAssembly);
        });

        services.AddValidatorsFromAssembly(applicationAssembly);


        services.AddScoped<IPasswordProvider, PasswordProvider>();
        services.AddScoped<ITokenProvider, JwtBearerProvider>();
        services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
        services.AddScoped<IUserMapper, UserMapper>();

        services.AddDbContext<AuthenticationContext>(options => options.UseSqlServer(configuration.GetConnectionString("Authentication")));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddAuthenticationJwt(configuration["Jwt:SecretKey"] ?? "");
        services.AddAuthorization(options =>
        {
            options.AddPolicy("AddUser", policy => policy.RequireRole("AddUser"));
        });

        return services;
    }
}
