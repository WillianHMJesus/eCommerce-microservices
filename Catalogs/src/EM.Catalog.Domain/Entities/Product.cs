namespace EM.Catalog.Domain.Entities;

public class Product : Entity, IAggregateRoot
{
    public const string ErrorMessageNameNullOrEmpty = "The product name cannot be null or empty.";
    public const string ErrorMessageDescriptionNullOrEmpty = "The product description cannot be null or empty.";
    public const string ErrorMessageValueLessThanEqualToZero = "The product value cannot be less than or equal to zero.";
    public const string ErrorMessageQuantityLessThanEqualToZero = "The product quantity cannot be less than or equal to zero.";
    public const string ErrorMessageImageNullOrEmpty = "The product image cannot be null or empty.";
    public const string ErrorMessageQuantityDebitedLessThanOrEqualToZero = "The quantity debited cannot be less than or equal to zero.";
    public const string ErrorMessageQuantityDebitedLargerThanAvailable = "The quantity debited cannot be larger than available.";
    public const string ErrorMessageQuantityAddedLessThanOrEqualToZero = "The quantity added cannot be less than or equal to zero.";
    public const string ErrorMessageCategoryNull = "";

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
        AssertionConcern.ValidateNullOrEmpty(Name, ErrorMessageNameNullOrEmpty);
        AssertionConcern.ValidateNullOrEmpty(Description, ErrorMessageDescriptionNullOrEmpty);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, ErrorMessageValueLessThanEqualToZero);
        AssertionConcern.ValidateLessThanEqualToMinimum(Quantity, 0, ErrorMessageQuantityLessThanEqualToZero);
        AssertionConcern.ValidateNullOrEmpty(Image, ErrorMessageImageNullOrEmpty);
    }

    public void Enable() => Active = true;

    public void Disable() => Active = false;

    public void RemoveQuantity(short quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, ErrorMessageQuantityDebitedLessThanOrEqualToZero);
        AssertionConcern.ValidateLessThanMinimum(Quantity, quantity, ErrorMessageQuantityDebitedLargerThanAvailable);
        Quantity -= quantity;
    }

    public void AddQuantity(short quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, ErrorMessageQuantityAddedLessThanOrEqualToZero);
        Quantity += quantity;
    }

    public void AddCategory(Category category)
    {
        AssertionConcern.ValidateNull(category, ErrorMessageCategoryNull);
        Category = category;
    }
}
