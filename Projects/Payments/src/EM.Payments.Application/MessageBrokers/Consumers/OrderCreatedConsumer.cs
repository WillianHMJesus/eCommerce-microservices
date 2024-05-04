using EM.Payments.Application.DTOs;
using EM.Payments.Application.Interfaces;
using EM.Payments.Domain.Entities;
using EM.Payments.Domain.Interfaces;
using EM.Shared.Core.MessageBrokers.Contracts;
using MassTransit;

namespace EM.Payments.Application.MessageBrokers.Consumers;

public sealed class OrderCreatedConsumer : IConsumer<OrderCreated>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMessageBrokerService _messageBrokerService;

    public OrderCreatedConsumer(
        IPaymentGateway paymentGateway,
        ITransactionRepository transactionRepository,
        IMessageBrokerService messageBrokerService)
    {
        _paymentGateway = paymentGateway;
        _transactionRepository = transactionRepository;
        _messageBrokerService = messageBrokerService;
    }

    public async Task Consume(ConsumeContext<OrderCreated> context)
    {
        OrderCreated orderCreated = context.Message;
        bool paymentResult = await _paymentGateway.ProccessPaymentAsync((PaymentDTO)orderCreated);

        Transaction transaction = new(orderCreated.UserId, orderCreated.OrderId, orderCreated.Value, orderCreated.CardNumber, paymentResult);
        await _transactionRepository.AddAsync(transaction);

        if (!transaction.PaymentAproved)
        {
            await SendMessagePaymentRefused(orderCreated);
            return;
        }

        await SendMessagePaymentApproved(orderCreated);
    }

    private async Task SendMessagePaymentApproved(OrderCreated orderCreated)
    {
        await _messageBrokerService.SendMessage(new PaymentApproved
        {
            UserId = orderCreated.UserId,
            OrderId = orderCreated.OrderId
        });
    }

    private async Task SendMessagePaymentRefused(OrderCreated orderCreated)
    {
        await _messageBrokerService.SendMessage(new PaymentRefused
        {
            UserId = orderCreated.UserId,
            OrderId = orderCreated.OrderId
        });
    }
}
