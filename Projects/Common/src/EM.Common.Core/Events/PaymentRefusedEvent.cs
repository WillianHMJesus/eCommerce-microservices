namespace EM.Common.Core.Events;

public sealed record PaymentRefusedEvent(Guid OrderId)
    : IntegrationEvent
{ }
