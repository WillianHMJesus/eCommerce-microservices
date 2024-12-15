using EM.Common.Core.Domain;
using EM.Common.Core.ResourceManagers;

namespace EM.Checkout.Domain.Entities;

public class Item : Entity
{
    public Item(Guid productId, string productName, string productImage, short quantity, decimal value)
    {
        ProductId = productId;
        ProductName = productName;
        ProductImage = productImage;
        Quantity = quantity;
        Value = value;

        Validate();
    }

    public Guid ProductId { get; init; }
    public string ProductName { get; init; }
    public string ProductImage { get; init; }
    public short Quantity { get; init; }
    public decimal Value { get; init; }
    public decimal Amount => Value * Quantity;

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrDefault(ProductId, Key.ProductInvalidId);
        AssertionConcern.ValidateNullOrEmpty(ProductName, Key.ProductNameNullOrEmpty);
        AssertionConcern.ValidateNullOrEmpty(ProductImage, Key.ProductNameNullOrEmpty);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, Key.ProductValueLessThanEqualToZero);
        AssertionConcern.ValidateLessThanEqualToMinimum(Quantity, 0, Key.ProductQuantityLessThanEqualToZero);
    }
}
