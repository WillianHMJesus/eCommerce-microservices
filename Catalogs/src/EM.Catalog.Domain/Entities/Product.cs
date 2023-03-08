namespace EM.Catalog.Domain.Entities;

public class Product : Entity
{
    public Product(string name, string description, decimal value, short quantity, string image)
    {
        Name = name;
        Description = description;
        Value = value;
        Quantity = quantity;
        Image = image;
        Active = true;

        Validate();
    }

    public string Name { get; init; }
    public string Description { get; init; }
    public decimal Value { get; init; }
    public short Quantity { get; private set; }
    public string Image { get; init; }
    public bool Active { get; private set; }
    public Category? Category { get; private set; }

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrEmpty(Name, ErrorMessage.ProductNameNullOrEmpty);
        AssertionConcern.ValidateNullOrEmpty(Description, ErrorMessage.ProductDescriptionNullOrEmpty);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, ErrorMessage.ProductValueLessThanEqualToZero);
        AssertionConcern.ValidateLessThanEqualToMinimum(Quantity, 0, ErrorMessage.ProductQuantityLessThanEqualToZero);
        AssertionConcern.ValidateNullOrEmpty(Image, ErrorMessage.ProductImageNullOrEmpty);
    }

    public void Enable() => Active = true;

    public void Disable() => Active = false;

    public void RemoveQuantity(short quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, ErrorMessage.ProductQuantityDebitedLessThanOrEqualToZero);
        AssertionConcern.ValidateLessThanMinimum(Quantity, quantity, ErrorMessage.ProductQuantityDebitedLargerThanAvailable);
        Quantity -= quantity;
    }

    public void AddQuantity(short quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, ErrorMessage.ProductQuantityAddedLessThanOrEqualToZero);
        Quantity += quantity;
    }

    public void AddCategory(Category category)
    {
        AssertionConcern.ValidateNull(category, ErrorMessage.ProductCategoryNull);
        Category = category;
    }
}
