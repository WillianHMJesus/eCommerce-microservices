using MassTransit;

namespace EM.Checkout.Orchestrator.Sagas.CreateOrder;

public sealed class CreateOrderState : SagaStateMachineInstance, ISagaVersion
{
    public Guid CorrelationId { get; set; }
    public long OrderId { get; set; }
    public string CurrentState { get; internal set; } = ""!;
    public int Version { get; set; }

    public List<CorrelatedBy<Guid>> Events { get; set; } = new List<CorrelatedBy<Guid>>();
}
