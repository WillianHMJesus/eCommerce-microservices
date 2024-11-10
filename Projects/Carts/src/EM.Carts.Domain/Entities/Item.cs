using EM.Common.Core.Domain;
using EM.Common.Core.ResourceManagers;

namespace EM.Carts.Domain.Entities;

public sealed class Item : Entity
{
    public Item(Guid productId, string productName, string productImage, decimal value, int quantity)
    {
        ProductId = productId;
        ProductName = productName;
        ProductImage = productImage;
        Value = value;
        Quantity = quantity;

        Validate();
    }

    public Guid ProductId { get; init; }
    public string ProductName { get; init; }
    public string ProductImage { get; init; }
    public decimal Value { get; init; }
    public int Quantity { get; private set; }

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrDefault(ProductId, Key.ProductInvalidId);
        AssertionConcern.ValidateNullOrEmpty(ProductName, Key.ProductNameNullOrEmpty);
        AssertionConcern.ValidateNullOrEmpty(ProductImage, Key.ProductImageNullOrEmpty);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, Key.ProductValueLessThanEqualToZero);
        AssertionConcern.ValidateLessThanEqualToMinimum(Quantity, 0, Key.ProductQuantityLessThanEqualToZero);
    }

    public void AddQuantity(int quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, Key.ProductQuantityLessThanEqualToZero);
        Quantity += quantity;
    }

    public void RemoveQuantity(int quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, Key.ProductQuantityLessThanEqualToZero);
        AssertionConcern.ValidateLessThanEqualToMinimum(Quantity - quantity, 0, Key.QuantityGreaterThanAvailable);
        Quantity -= quantity;
    }
}
