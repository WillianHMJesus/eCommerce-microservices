namespace EM.Catalog.Domain.Entities;

public class Product : Entity
{
    public Product(string name, string description, decimal value, int quantity, string image)
    {
        Name= name;
        Description= description;
        Value= value;
        Quantity= quantity;
        Image= image;
        Active = true;
    }

    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Value { get; init; }
    public int Quantity { get; private set; }
    public string Image { get; init; }
    public bool Active { get; init; }

    public void DebitQuantity(int quantidade)
    {
        Quantity -= quantidade;
    }
}
