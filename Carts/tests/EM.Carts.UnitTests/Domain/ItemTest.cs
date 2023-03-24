using EM.Carts.Domain;
using EM.Carts.Domain.Entities;
using EM.Carts.UnitTests.Fixtures;
using System;
using Xunit;

namespace EM.Carts.UnitTests.Domain;

public class ItemTest
{
    private readonly ItemFixture _itemFixture;

    public ItemTest()
    {
        _itemFixture = new ItemFixture();
    }

    [Fact]
    public void Validate_ValidItem_MustNotReturnDomainException()
    {
        Exception domainException = Record.Exception(() 
            => _itemFixture.GenerateValidItem());

        Assert.Null(domainException);
    }

    [Fact]
    public void Validate_InvalidProductId_MustReturnDomainException()
    {
        Exception domainException = Record.Exception(()
            => _itemFixture.GenerateItemWithInvalidProductId());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductIdInvalid, domainException.Message);
    }

    [Fact]
    public void Validate_WithoutProductName_MustReturnDomainException()
    {
        Exception domainException = Record.Exception(()
            => _itemFixture.GenerateItemWithoutProductName());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductNameNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_WithoutProductImage_MustReturnDomainException()
    {
        Exception domainException = Record.Exception(()
            => _itemFixture.GenerateItemWithoutProductImage());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ProductImageNullOrEmpty, domainException.Message);
    }

    [Fact]
    public void Validate_InvalidValue_MustReturnDomainException()
    {
        Exception domainException = Record.Exception(()
            => _itemFixture.GenerateItemWithInvalidValue());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.ValueLessThanEqualToZero, domainException.Message);
    }

    [Fact]
    public void Validate_InvalidQuantity_MustReturnDomainException()
    {
        Exception domainException = Record.Exception(()
            => _itemFixture.GenerateItemWithInvalidQuantity());

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.QuantityLessThanEqualToZero, domainException.Message);
    }

    [Fact]
    public void AddQuantity_ValidQuantity_MustAddQuantityToItem()
    {
        Item item = _itemFixture.GenerateValidItem();
        int itemQuantity = item.Quantity;

        Exception domainException = Record.Exception(()
            => item.AddQuantity(1));

        Assert.Equal(itemQuantity + 1, item.Quantity);
        Assert.Null(domainException);
    }

    [Fact]
    public void AddQuantity_InvalidQuantity_MustReturnDomainException()
    {
        Item item = _itemFixture.GenerateValidItem();

        Exception domainException = Record.Exception(()
            => item.AddQuantity(0));

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.QuantityLessThanEqualToZero, domainException.Message);
    }

    [Fact]
    public void SubtractQuantity_ValidQuantity_MustSubtractQuantityToItem()
    {
        Item item = _itemFixture.GenerateValidItem();
        int itemQuantity = item.Quantity;

        Exception domainException = Record.Exception(()
            => item.SubtractQuantity(1));

        Assert.Equal(itemQuantity - 1, item.Quantity);
        Assert.Null(domainException);
    }

    [Fact]
    public void SubtractQuantity_InvalidQuantity_MustReturnDomainException()
    {
        Item item = _itemFixture.GenerateValidItem();

        Exception domainException = Record.Exception(()
            => item.SubtractQuantity(0));

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.QuantityLessThanEqualToZero, domainException.Message);
    }

    [Fact]
    public void SubtractQuantity_QuantityEqualThanExisting_MustReturnDomainException()
    {
        Item item = _itemFixture.GenerateValidItem();

        Exception domainException = Record.Exception(()
            => item.SubtractQuantity(item.Quantity));

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.QuantityLessThanEqualToZero, domainException.Message);
    }

    [Fact]
    public void SubtractQuantity_QuantityGreaterThanExisting_MustReturnDomainException()
    {
        Item item = _itemFixture.GenerateValidItem();

        Exception domainException = Record.Exception(()
            => item.SubtractQuantity(item.Quantity + 1));

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.QuantityLessThanEqualToZero, domainException.Message);
    }
}
