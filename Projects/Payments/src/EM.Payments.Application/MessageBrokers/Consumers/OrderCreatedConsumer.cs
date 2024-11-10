using AutoMapper;
using EM.Common.Core.Events;
using EM.Common.Core.MessageBrokers;
using EM.Payments.Application.Interfaces;
using EM.Payments.Domain.Entities;
using EM.Payments.Domain.Interfaces;
using MassTransit;

namespace EM.Payments.Application.MessageBrokers.Consumers;

public sealed class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
{
    private readonly IPaymentGateway _paymentGateway;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IMessageBrokerService _messageBrokerService;
    private readonly IMapper _mapper;

    public OrderCreatedConsumer(
        IPaymentGateway paymentGateway,
        ITransactionRepository transactionRepository,
        IMessageBrokerService messageBrokerService,
        IMapper mapper)
    {
        _paymentGateway = paymentGateway;
        _transactionRepository = transactionRepository;
        _messageBrokerService = messageBrokerService;
        _mapper = mapper;
    }

    public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
    {
        OrderCreatedEvent message = context.Message;
        bool paymentResult = _paymentGateway.ProccessPayment(message);

        Transaction transaction = _mapper.Map<Transaction>((message, paymentResult));
        await _transactionRepository.AddAsync(transaction);

        if (paymentResult)
        {
            await _messageBrokerService.SendMessage(new PaymentApprovedEvent(message.OrderId), context.CancellationToken);
            return;
        }

        await _messageBrokerService.SendMessage(new PaymentRefusedEvent(message.OrderId), context.CancellationToken);
    }
}
