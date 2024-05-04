using EM.Shared.Core.MessageBrokers.Contracts;

namespace EM.Payments.Application.DTOs;

public sealed record PaymentDTO
{
    public Guid UserId { get; set; }
    public Guid OrderId { get; set; }
    public decimal Value { get; set; }
    public string CardHolderCpf { get; set; } = ""!;
    public string CardHolderName { get; set; } = ""!;
    public string CardNumber { get; set; } = ""!;
    public string CardExpirationDate { get; set; } = ""!;
    public string CardSecurityCode { get; set; } = ""!;

    public static explicit operator PaymentDTO(OrderCreated orderCreated)
    {
        return new PaymentDTO
        {
            UserId = orderCreated.UserId,
            OrderId = orderCreated.OrderId,
            Value = orderCreated.Value,
            CardHolderCpf = orderCreated.CardHolderCpf,
            CardHolderName = orderCreated.CardHolderName,
            CardNumber = orderCreated.CardNumber,
            CardExpirationDate = orderCreated.CardExpirationDate,
            CardSecurityCode = orderCreated.CardSecurityCode
        };
    }
}
