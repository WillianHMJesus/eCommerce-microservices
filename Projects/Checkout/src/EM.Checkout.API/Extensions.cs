using EM.Checkout.Application;
using EM.Checkout.Application.Interfaces.ExternalServices;
using EM.Checkout.Application.MessageBrokers;
using EM.Checkout.Application.MessageBrokers.Consumers;
using EM.Checkout.Domain.Interfaces;
using EM.Checkout.Infraestructure.ExternalServices;
using EM.Checkout.Infraestructure.Persistense;
using EM.Checkout.Infraestructure.Persistense.Repositories;
using EM.Common.Core.MessageBrokers;
using FluentValidation;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EM.Checkout.API;

public static class Extensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<PaymentApprovedConsumer>();
            x.AddConsumer<PaymentRefusedConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(configuration.GetConnectionString("RabbitMq"));

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AssemblyReference.Assembly));
        services.AddAutoMapper(AssemblyReference.Assembly);
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddDbContext<CheckoutContext>(options => options.UseSqlServer(configuration.GetConnectionString("Checkout")));
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IMessageBrokerService, MassTransitService>();

        services.AddHttpClient<ICartExternalService, CartExternalService>(client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("ExternalServices:Cart:BaseAddress") 
                ?? throw new ArgumentNullException());
        });

        return services;
    }
}
