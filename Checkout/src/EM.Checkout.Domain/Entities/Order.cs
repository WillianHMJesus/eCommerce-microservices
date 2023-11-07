using EM.Checkout.Domain.Entities.Enums;
using EM.Shared.Core;

namespace EM.Checkout.Domain.Entities;

public class Order : Entity
{
    public Order(Guid userId)
    {
        UserId = userId;
        OrderStatus = OrderStatus.Created;
        Items = new List<Item>();
    }

    public Guid UserId { get; init; }
    public OrderStatus OrderStatus { get; private set; }
    public List<Item> Items { get; init; }
    public decimal Amount => Items.Sum(x => x.Amount);

    public override void Validate()
    {
        AssertionConcern.ValidateDefault(UserId, ErrorMessage.UserIdInvalid);
    }

    public void AddItem(Item item)
    {
        AssertionConcern.ValidateNull(item, ErrorMessage.OrderItemNull);
        Items.Add(item);
    }
}
