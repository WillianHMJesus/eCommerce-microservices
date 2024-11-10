using AutoFixture.Xunit2;
using EM.Common.Core.Events;
using EM.Common.Core.MessageBrokers;
using EM.Payments.Application.Interfaces;
using EM.Payments.Application.MessageBrokers.Consumers;
using EM.Payments.UnitTests.Application.CustomAutoData;
using MassTransit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EM.Payments.UnitTests.Application.MessageBrokers.Consumers;

public sealed class OrderCreatedConsumerTest
{
    [Theory, AutoTransactionData]
    public async Task Consume_PaymentApproved_ShouldInvokePaymentApprovedEvent(
        [Frozen] Mock<IPaymentGateway> gatewayMock,
        [Frozen] Mock<IMessageBrokerService> serviceMock,
        OrderCreatedConsumer sut,
        ConsumeContext<OrderCreatedEvent> context)
    {
        gatewayMock
            .Setup(x => x.ProccessPayment(It.IsAny<OrderCreatedEvent>()))
            .Returns(true);

        await sut.Consume(context);

        serviceMock.Verify(x => x.SendMessage(It.IsAny<PaymentApprovedEvent>(), It.IsAny<CancellationToken>()));
    }

    [Theory, AutoTransactionData]
    public async Task Consume_PaymentRefused_ShouldInvokePaymentRefusedEvent(
        [Frozen] Mock<IPaymentGateway> gatewayMock,
        [Frozen] Mock<IMessageBrokerService> serviceMock,
        OrderCreatedConsumer sut,
        ConsumeContext<OrderCreatedEvent> context)
    {
        gatewayMock
            .Setup(x => x.ProccessPayment(It.IsAny<OrderCreatedEvent>()))
            .Returns(false);

        await sut.Consume(context);

        serviceMock.Verify(x => x.SendMessage(It.IsAny<PaymentRefusedEvent>(), It.IsAny<CancellationToken>()));
    }
}
