﻿using EM.Shared.Core;

namespace EM.Catalog.Domain.Entities;

public class Product : Entity
{
    public Product(string name, string description, decimal value, string image)
    {
        Name = name;
        Description = description;
        Value = value;
        Image = image;
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
    public Category? Category { get; private set; }

    public override void Validate()
    {
        AssertionConcern.ValidateNullOrEmpty(Name, ErrorMessage.ProductNameNullOrEmpty);
        AssertionConcern.ValidateNullOrEmpty(Description, ErrorMessage.ProductDescriptionNullOrEmpty);
        AssertionConcern.ValidateLessThanEqualToMinimum(Value, 0, ErrorMessage.ProductValueLessThanEqualToZero);
        AssertionConcern.ValidateNullOrEmpty(Image, ErrorMessage.ProductImageNullOrEmpty);
    }

    public void MakeAvailable() => Available = true;

    public void MakeUnavailable() => Available = false;

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

    public void AssignCategory(Category category)
    {
        AssertionConcern.ValidateNull(category, ErrorMessage.ProductCategoryNull);
        Category = category;
    }
}
