using EM.Payments.Application.DTOs;
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

    public Task<bool> ProccessPaymentAsync(PaymentDTO payment)
    {
        return Task.FromResult(cardsNumber.Any(x => x == payment.CardNumber));
    }
}
