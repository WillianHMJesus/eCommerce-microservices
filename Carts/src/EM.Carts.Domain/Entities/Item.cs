namespace EM.Carts.Domain.Entities;

public class Item : Entity
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

    public Guid ProductId { get; set; }
    public string ProductName { get; set; }
    public string ProductImage { get; set; }
    public decimal Value { get; set; }
    public int Quantity { get; set; }

    public override void Validate()
    {
        AssertionConcern.ValidateNull(ProductId, ErrorMessage.ProductIdInvalid);
        AssertionConcern.ValidateNullOrEmpty(ProductName, ErrorMessage.ProductNameNullOrEmpty);
        AssertionConcern.ValidateNullOrEmpty(ProductImage, ErrorMessage.ProductImageNullOrEmpty);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, ErrorMessage.ValueLessThanEqualToZero);
        AssertionConcern.ValidateLessThanEqualToMinimum(Quantity, 0, ErrorMessage.QuantityLessThanEqualToZero);
    }

    public void AddQuantity(int quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, ErrorMessage.QuantityLessThanEqualToZero);
        Value += quantity;
    }

    public void SubtractQuantity(int quantity)
    {
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, ErrorMessage.QuantityLessThanEqualToZero);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value -= quantity, 0, ErrorMessage.QuantityLessThanEqualToZero);

        Value -= quantity;

    }
}
