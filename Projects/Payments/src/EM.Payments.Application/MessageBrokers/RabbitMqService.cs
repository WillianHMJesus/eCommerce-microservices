using MassTransit;

namespace EM.Payments.Application.MessageBrokers;

public sealed class RabbitMqService : IMessageBrokerService
{
    private readonly IBus _bus;

    public RabbitMqService(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendMessage<T>(T message, CancellationToken cancellationToken = default(CancellationToken)) where T : class
    {
        await _bus.Publish(message, cancellationToken);
    }
}
