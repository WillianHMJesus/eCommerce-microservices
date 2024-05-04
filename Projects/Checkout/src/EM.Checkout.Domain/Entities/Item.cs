using EM.Shared.Core;

namespace EM.Checkout.Domain.Entities;

public class Item : Entity
{
    public Item(Guid productId, string productName, string productImage, int quantity, decimal value)
    {
        ProductId = productId;
        ProductName = productName;
        ProductImage = productImage;
        Quantity = quantity;
        Value = value;
    }

    public Guid ProductId { get; init; }
    public string ProductName { get; init; }
    public string ProductImage { get; init; }
    public int Quantity { get; init; }
    public decimal Value { get; init; }
    public decimal Amount => Value * Quantity;

    public override void Validate()
    {
        AssertionConcern.ValidateDefault(ProductId, ErrorMessage.ProductIdInvalid);
        AssertionConcern.ValidateNullOrEmpty(ProductName, ErrorMessage.ProductNameNullOrEmpty);
        AssertionConcern.ValidateNullOrEmpty(ProductImage, ErrorMessage.ProductNameNullOrEmpty);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, ErrorMessage.ValueLessThanEqualToZero);
        AssertionConcern.ValidateLessThanEqualToMinimum(Quantity, 0, ErrorMessage.QuantityLessThanEqualToZero);
    }
}
