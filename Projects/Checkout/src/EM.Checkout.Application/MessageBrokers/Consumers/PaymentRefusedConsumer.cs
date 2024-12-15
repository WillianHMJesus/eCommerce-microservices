using EM.Checkout.Domain.Entities;
using EM.Checkout.Domain.Interfaces;
using EM.Common.Core.Events;
using MassTransit;

namespace EM.Checkout.Application.MessageBrokers.Consumers;

public sealed class PaymentRefusedConsumer : IConsumer<PaymentRefusedEvent>
{
    private readonly IOrderRepository _repository;

    public PaymentRefusedConsumer(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<PaymentRefusedEvent> context)
    {
        PaymentRefusedEvent message = context.Message;
        Order? order = await _repository.GetByIdAsync(message.OrderId, context.CancellationToken);

        if (order == null) return;

        order.RefuseOrder();
        await _repository.UpdateAsync(order, context.CancellationToken);
    }
}
