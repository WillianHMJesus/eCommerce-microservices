using EM.Shared.Core;

namespace EM.Payments.Domain.Entities;

public sealed class Transaction : Entity
{
    public Transaction(Guid userId, Guid orderId, decimal value, string cardNumber, bool paymentAproved)
    {
        UserId = userId;
        OrderId = orderId;
        Value = value;
        CardNumber = cardNumber;
        PaymentAproved = paymentAproved;
    }

    public Guid UserId { get; init; }
    public Guid OrderId { get; init; }
    public decimal Value { get; set; }
    public string CardNumber { get; init; }
    public bool PaymentAproved { get; set; }

    public override void Validate()
    {
        AssertionConcern.ValidateDefault(UserId, ErrorMessage.UserIdInvalid);
        AssertionConcern.ValidateDefault(OrderId, ErrorMessage.OrderIdInvalid);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, ErrorMessage.ValueInvalid);
        AssertionConcern.ValidateNullOrEmpty(CardNumber, ErrorMessage.CardNumberNullOrEmpty);
    }
}
