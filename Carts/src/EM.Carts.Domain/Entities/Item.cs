using EM.Shared.Core;

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
        AssertionConcern.ValidateDefault(ProductId, ErrorMessage.ProductIdInvalid);
        AssertionConcern.ValidateNullOrEmpty(ProductName, ErrorMessage.ProductNameNullOrEmpty);
        AssertionConcern.ValidateNullOrEmpty(ProductImage, ErrorMessage.ProductImageNullOrEmpty);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, ErrorMessage.ValueLessThanEqualToZero);
        AssertionConcern.ValidateLessThanEqualToMinimum(Quantity, 0, ErrorMessage.QuantityLessThanEqualToZero);
    }

    public void AddQuantity(int quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, ErrorMessage.QuantityLessThanEqualToZero);
        Quantity += quantity;
    }

    public void SubtractQuantity(int quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(quantity, 0, ErrorMessage.QuantityLessThanEqualToZero);
        AssertionConcern.ValidateLessThanEqualToMinimum(Quantity - quantity, 0, ErrorMessage.QuantityGreaterThanAvailable);

        Quantity -= quantity;
    }
}
