using EM.Common.Core.Domain;
using EM.Common.Core.ResourceManagers;

namespace EM.Catalog.Domain.Entities;

public class Product : Entity
{
    public Product(string name, string description, decimal value, string image, Guid categoryId)
    {
        Name = name;
        Description = description;
        Value = value;
        Image = image;
        CategoryId = categoryId;
        Quantity = 0;
        Available = true;

        Validate();
    }

    public string Name { get; init; } = ""!;
    public string Description { get; init; } = ""!;
    public decimal Value { get; init; }
    public short Quantity { get; private set; }
    public string Image { get; init; } = ""!;
    public bool Available { get; private set; }
    public Guid CategoryId { get; init; }
    public Category? Category { get; private set; }

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrEmpty(Name, Key.ProductNameNullOrEmpty);
        AssertionConcern.ValidateNullOrEmpty(Description, Key.ProductDescriptionNullOrEmpty);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, Key.ProductValueLessThanEqualToZero);
        AssertionConcern.ValidateNullOrEmpty(Image, Key.ProductImageNullOrEmpty);
        AssertionConcern.ValidateNullOrDefault(CategoryId, Key.ProductInvalidCategoryId);
    }

    public void MakeAvailable() => Available = true;

    public void MakeUnavailable() => Available = false;

    public void RemoveQuantity(short quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, Key.ProductQuantityDebitedLessThanOrEqualToZero);
        AssertionConcern.ValidateLessThanMinimum(Quantity, quantity, Key.ProductQuantityDebitedLargerThanAvailable);
        Quantity -= quantity;
    }

    public void AddQuantity(short quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, Key.ProductQuantityAddedLessThanOrEqualToZero);
        Quantity += quantity;
    }
}
