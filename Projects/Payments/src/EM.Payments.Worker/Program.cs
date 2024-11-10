using EM.Payments.Worker;
using MassTransit;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddMassTransit(ctx.Configuration);
        services.AddDependencyInjection(ctx.Configuration);
    })
    .Build();

await host.RunAsync();
