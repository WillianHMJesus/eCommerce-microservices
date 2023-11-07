using MassTransit;

namespace EM.Checkout.Orchestrator.Events;

public sealed record PaymentUnauthorized : CorrelatedBy<Guid>
{
    public long OrderId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public Guid CorrelationId { get; set; }
}