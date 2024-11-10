using EM.Common.Core.Events;

namespace EM.Payments.Application.Interfaces;

public interface IPaymentGateway
{
    bool ProccessPayment(OrderCreatedEvent _event);
}
