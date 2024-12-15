using EM.Common.Core.Events;
using EM.Common.Core.MessageBrokers;
using MassTransit;

namespace EM.Checkout.Application.MessageBrokers;

public sealed class MassTransitService : IMessageBrokerService
{
    private readonly IBus _bus;

    public MassTransitService(IBus bus)
    {
        _bus = bus;
    }

    public async Task SendMessage<T>(T message, CancellationToken cancellationToken) where T : IntegrationEvent
    {
        await _bus.Publish(message, cancellationToken);
    }
}
