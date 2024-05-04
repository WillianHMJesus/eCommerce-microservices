using MassTransit;

namespace EM.Checkout.Orchestrator.Events;

public sealed record OrderCreated : CorrelatedBy<Guid>
{
    public long OrderId { get; set; }
    public DateTime CreatedDateTime { get; set; }
    public Guid CorrelationId { get; set; }
}
