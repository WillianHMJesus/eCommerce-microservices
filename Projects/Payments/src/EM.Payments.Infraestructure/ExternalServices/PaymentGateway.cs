using EM.Common.Core.Events;
using EM.Payments.Application.Interfaces;

namespace EM.Payments.Infraestructure.ExternalServices;

public sealed class PaymentGateway : IPaymentGateway
{
    private static IList<string> cardsNumber = new List<string>
    {
        "0000000000000000",
        "0000000000000001",
        "0001000100010001",
        "1234456778904321"
    };

    public bool ProccessPayment(OrderCreatedEvent _event)
    {
        return !cardsNumber.Any(x => x == _event.CardNumber);
    }
}
