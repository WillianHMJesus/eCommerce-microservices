﻿namespace EM.Carts.Domain.Entities;

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

    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public string ProductImage { get; private set; }
    public decimal Value { get; private set; }
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
        AssertionConcern.ValidateLessThanEqualToMinimum(Quantity - quantity, 0, ErrorMessage.QuantityLessThanEqualToZero);

        Quantity -= quantity;
    }
}
