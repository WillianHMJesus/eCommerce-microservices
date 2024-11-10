using AutoFixture.Xunit2;
using EM.Common.Core.Events;
using EM.Payments.Application.MessageBrokers;
using EM.Payments.UnitTests.Application.CustomAutoData;
using MassTransit;
using Moq;

namespace EM.Payments.UnitTests.Application.MessageBrokers;

public class MassTransitServiceTest
{
    [Theory, AutoTransactionData]
    public async Task SendMessage_MessageIsValid_ShouldInvokePublish(
        [Frozen] Mock<IBus> busMock,
        [Frozen] Mock<IntegrationEvent> eventMock,
        MassTransitService sut)
    {
        await sut.SendMessage(eventMock.Object, CancellationToken.None);

        busMock.Verify(x => x.Publish(It.IsAny<object>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
