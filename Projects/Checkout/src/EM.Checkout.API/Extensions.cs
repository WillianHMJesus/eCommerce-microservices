using EM.Checkout.Application.Interfaces;
using EM.Checkout.Application.MessageBrokers;
using EM.Checkout.Application.UseCases.Purchase;
using EM.Checkout.Application.UseCases.Purchase.Validations;
using EM.Checkout.Domain.Interfaces;
using EM.Checkout.Infraestructure.ExternalServices;
using EM.Checkout.Infraestructure.Persistense;
using EM.Checkout.Infraestructure.Persistense.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EM.Checkout.API;

public static class Extensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
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
        services.AddDbContext<CheckoutContext>(options => options.UseSqlServer(configuration.GetConnectionString("Checkout")));
        services.AddScoped<CheckoutContext>();
        services.AddScoped<IOrderRepository, OrderRepository>();

        services.AddScoped<IPurchaseUseCase, PurchaseUseCase>();
        services.Decorate<IPurchaseUseCase, PurchaseRequestValidation>();

        services.AddScoped<ICartExternalService, CartExternalService>();

        services.AddScoped<IMessageBrokerService, RabbitMqService>();

        services.AddHttpClient("Cart", client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("ExternalServices:Cart:BaseAddress"));
        });

        return services;
    }
}
