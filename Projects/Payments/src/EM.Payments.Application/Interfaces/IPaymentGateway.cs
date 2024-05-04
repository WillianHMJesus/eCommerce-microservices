using EM.Payments.Application.DTOs;

namespace EM.Payments.Application.Interfaces;

public interface IPaymentGateway
{
    Task<bool> ProccessPaymentAsync(PaymentDTO payment);
}
