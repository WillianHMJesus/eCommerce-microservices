namespace EM.Payments.Application.MessageBrokers;

public interface IMessageBrokerService
{
    Task SendMessage<T>(T message, CancellationToken cancellationToken = default(CancellationToken)) where T : class;
}