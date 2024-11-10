using EM.Common.Core.Domain;
using EM.Common.Core.ResourceManagers;

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
        AssertionConcern.ValidateNullOrDefault(UserId, Key.UserIdInvalid);
    }

    public void AddItem(Item item)
    {
        AssertionConcern.ValidateNullOrDefault(item, Key.CartItemNull);
        Items.Add(item);
    }

    public void RemoveItem(Item item)
    {
        AssertionConcern.ValidateNullOrDefault(item, Key.CartItemNull);
        Items.Remove(item);
    }

    public void RemoveAllItems()
    {
        Items.Clear();
    }
}
