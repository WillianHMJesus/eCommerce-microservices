using MassTransit;
using EM.Common.Core.Events;
using EM.Checkout.Domain.Entities;
using EM.Checkout.Domain.Interfaces;

namespace EM.Checkout.Application.MessageBrokers.Consumers;

public sealed class PaymentApprovedConsumer : IConsumer<PaymentApprovedEvent>
{
    private readonly IOrderRepository _repository;

    public PaymentApprovedConsumer(IOrderRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<PaymentApprovedEvent> context)
    {
        Guid orderId = context.Message.OrderId;
        Order? order = await _repository.GetByIdAsync(orderId, context.CancellationToken);

        if (order == null) return;

        order.PayOrder();
        await _repository.UpdateAsync(order, context.CancellationToken);
    }
}
