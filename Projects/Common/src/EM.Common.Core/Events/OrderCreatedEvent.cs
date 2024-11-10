namespace EM.Common.Core.Events;

public sealed record OrderCreatedEvent(Guid UserId, Guid OrderId, decimal Value, string CardHolderCpf, string CardHolderName, string CardNumber, string CardExpirationDate, string CardSecurityCode)
    : IntegrationEvent
{ }
