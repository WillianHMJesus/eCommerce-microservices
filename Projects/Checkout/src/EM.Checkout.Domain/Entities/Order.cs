using EM.Checkout.Domain.Entities.Enums;
using EM.Common.Core.Domain;
using EM.Common.Core.ResourceManagers;

namespace EM.Checkout.Domain.Entities;

public class Order : Entity
{
    public Order(Guid userId, string number)
    {
        UserId = userId;
        Number = number;
        OrderStatus = OrderStatus.Created;
        Items = new List<Item>();

        Validate();
    }

    public Guid UserId { get; init; }
    public string Number { get; init; }
    public OrderStatus OrderStatus { get; private set; }
    public List<Item> Items { get; init; }
    public decimal Amount => Items.Sum(x => x.Amount);

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrDefault(UserId, Key.UserIdInvalid);
        AssertionConcern.ValidateNullOrEmpty(Number, Key.OrderNumberNull);
    }

    public void AddItem(Item item)
    {
        AssertionConcern.ValidateNullOrDefault(item, Key.OrderItemNull);
        Items.Add(item);
    }

    public void PayOrder()
    {
        OrderStatus = OrderStatus.Paid;
    }

    public void RefuseOrder()
    {
        OrderStatus = OrderStatus.PaymentRefused;
    }
}
