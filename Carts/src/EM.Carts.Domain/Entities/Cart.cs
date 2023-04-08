namespace EM.Carts.Domain.Entities;

public class Cart : Entity
{
    public Cart(Guid userId)
    {
        UserId = userId;
        Items = new List<Item>();

        Validate();
    }

    public Guid UserId { get; init; }

    public List<Item> Items { get; private set; }

    public override void Validate()
    {
        AssertionConcern.ValidateDefault(UserId, ErrorMessage.UserIdInvalid);
    }

    public void AddItem(Item item)
    {
        AssertionConcern.ValidateNull(item, ErrorMessage.CartItemNull);
        Items.Add(item);
    }
}
