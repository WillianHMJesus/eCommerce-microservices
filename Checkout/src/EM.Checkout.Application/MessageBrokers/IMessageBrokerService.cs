namespace EM.Checkout.Application.MessageBrokers;

public interface IMessageBrokerService
{
    Task SendMessage<T>(T message, CancellationToken cancellationToken) where T : class;
}
