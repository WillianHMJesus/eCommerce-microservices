using EM.Checkout.Orchestrator.Events;
using MassTransit;

namespace EM.Checkout.Orchestrator.Sagas.CreateOrder;

public sealed class CreateOrderStateMachine : MassTransitStateMachine<CreateOrderState>
{
    public CreateOrderStateMachine()
    {
        InstanceState(x => x.CurrentState);

        Event(() => OrderCreated, context => context.CorrelateById(x => x.Message.CorrelationId));
        Event(() => PaymentAuthorized, context => context.CorrelateById(x => x.Message.CorrelationId));
        Event(() => PaymentUnauthorized, context => context.CorrelateById(x => x.Message.CorrelationId));

        Initially
        (
            When(OrderCreated)
                .Activity(x => x.OfType<OrderCreatedActivity>())
                .PublishAsync(AuthorizePaymentWhenPreOrderCreated)
                .TransitionTo(AuthorizingPayment)
        );
    }

    public Event<OrderCreated> OrderCreated { get; set; }
    public Event<PaymentAuthorized> PaymentAuthorized { get; set; }

    public Event<PaymentUnauthorized> PaymentUnauthorized { get; set; }
}
