using EM.Common.Core.MessageBrokers;
using EM.Payments.Application.Interfaces;
using EM.Payments.Application.Mappings;
using EM.Payments.Application.MessageBrokers;
using EM.Payments.Application.MessageBrokers.Consumers;
using EM.Payments.Domain.Interfaces;
using EM.Payments.Infraestructure.ExternalServices;
using EM.Payments.Infraestructure.Persistense;
using EM.Payments.Infraestructure.Persistense.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EM.Payments.Worker;

public static class Extensions
{
    public static IServiceCollection AddMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderCreatedConsumer>();

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
        services.AddAutoMapper(typeof(TransactionMapping));
        services.AddDbContext<PaymentContext>(options => options.UseSqlServer(configuration.GetConnectionString("Payment")));
        services.AddScoped<PaymentContext>();
        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IPaymentGateway, PaymentGateway>();
        services.AddScoped<IMessageBrokerService, MassTransitService>();

        return services;
    }
}
