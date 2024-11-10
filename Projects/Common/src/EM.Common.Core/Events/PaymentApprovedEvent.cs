namespace EM.Common.Core.Events;

public sealed record PaymentApprovedEvent(Guid OrderId)
    : IntegrationEvent
{ }
