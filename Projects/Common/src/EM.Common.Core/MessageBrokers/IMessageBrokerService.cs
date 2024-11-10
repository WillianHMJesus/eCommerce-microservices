using EM.Common.Core.Events;

namespace EM.Common.Core.MessageBrokers;

public interface IMessageBrokerService
{
    Task SendMessage<T>(T message, CancellationToken cancellationToken) where T : IntegrationEvent;
}
