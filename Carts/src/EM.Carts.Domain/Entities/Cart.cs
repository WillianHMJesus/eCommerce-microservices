using EM.Shared.Core;

namespace EM.Carts.Domain.Entities;

public sealed class Cart : Entity
{
    public Cart(Guid userId)
    {
        UserId = userId;
        Items = new List<Item>();

        Validate();
    }

    public Guid UserId { get; init; }

    public List<Item> Items { get; init; }

    public override void Validate()
    {
        AssertionConcern.ValidateDefault(UserId, ErrorMessage.UserIdInvalid);
    }

    public void AddItem(Item item)
    {
        AssertionConcern.ValidateNull(item, ErrorMessage.CartItemNull);
        Items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        AssertionConcern.ValidateNull(item, ErrorMessage.CartItemNull);
        Items.Remove(item);
    }

    public void RemoveAllItems()
    {
        Items.Clear();
    }
}
