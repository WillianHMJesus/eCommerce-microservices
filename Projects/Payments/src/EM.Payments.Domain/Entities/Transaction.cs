using EM.Common.Core.Domain;
using EM.Common.Core.ResourceManagers;

namespace EM.Payments.Domain.Entities;

public sealed class Transaction : Entity
{
    public Transaction(Guid userId, Guid orderId, decimal value, string cardNumber, string status)
    {
        UserId = userId;
        OrderId = orderId;
        Value = value;
        CardNumber = cardNumber;
        Status = status;
        Date = DateTime.Now;
    }

    public Guid UserId { get; init; }
    public Guid OrderId { get; init; }
    public decimal Value { get; set; }
    public string CardNumber { get; init; }
    public string Status { get; set; }
    public DateTime Date { get; set; }

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrDefault(UserId, Key.UserIdInvalid);
        AssertionConcern.ValidateNullOrDefault(OrderId, Key.OrderIdInvalid);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, Key.ValueInvalid);
        AssertionConcern.ValidateNullOrEmpty(CardNumber, Key.CardNumberInvalid);
        AssertionConcern.ValidateNullOrEmpty(Status, Key.StatusInvalid);
    }
}
