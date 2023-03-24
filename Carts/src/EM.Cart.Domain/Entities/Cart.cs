namespace EM.Cart.Domain.Entities;

public class Cart : Entity
{
    public Cart(Guid userId)
    {
        UserId = userId;
        _items = new List<Item>();
    }

    public Guid UserId { get; init; }
    
    private List<Item> _items;
    public IReadOnlyCollection<Item> Items => _items;

    public override void Validate()
    {
        AssertionConcern.ValidateNull(UserId, ErrorMessage.UserIdInvalid);
    }

    public void AddItem(Item item)
    {
        AssertionConcern.ValidateNull(item, ErrorMessage.CartItemNull);
        _items.Add(item);
    }
}
