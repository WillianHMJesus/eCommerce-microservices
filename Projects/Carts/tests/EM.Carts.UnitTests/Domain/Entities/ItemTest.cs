using AutoFixture;
using AutoFixture.Xunit2;
using EM.Carts.Domain.Entities;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using System;
using Xunit;

namespace EM.Carts.UnitTests.Domain.Entities;

public sealed class ItemTest
{
    private readonly Fixture _fixture;

    public ItemTest() => _fixture = new();

    [Theory, AutoData]
    public void Validate_ValidItem_ShouldNotReturnDomainException(Item item)
    {
        Exception domainException = Record.Exception(() => item.Validate());

        domainException.Should().BeNull();
    }

    [Fact]
    public void Validate_DefaultProductId_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.ProductId, Guid.Empty)
            .Create();

        Exception domainException = Record.Exception(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductInvalidId);
    }

    [Fact]
    public void Validate_NullProductName_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.ProductName, null as string)
            .Create();

        Exception domainException = Record.Exception(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Validate_EmptyProductName_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.ProductName, "")
            .Create();

        Exception domainException = Record.Exception(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Validate_NullProductImage_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.ProductImage, null as string)
            .Create();

        Exception domainException = Record.Exception(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductImageNullOrEmpty);
    }

    [Fact]
    public void Validate_EmptyProductImage_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.ProductImage, "")
            .Create();

        Exception domainException = Record.Exception(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductImageNullOrEmpty);
    }

    [Fact]
    public void Validate_ZeroValue_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.Value, 0)
            .Create();

        Exception domainException = Record.Exception(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductValueLessThanEqualToZero);
    }

    [Theory, AutoData]
    public void Validate_ZeroQuantity_ShouldReturnDomainException(Item item)
    {
        Exception domainException = Record.Exception(() 
            => new Item(item.ProductId, item.ProductName, item.ProductImage, item.Value, 0));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductQuantityLessThanEqualToZero);
    }

    [Theory, AutoData]
    public void AddQuantity_ValidQuantity_ShouldAddQuantity(Item item, int quantity)
    {
        int finalQuantity = item.Quantity + quantity;

        item.AddQuantity(quantity);

        item.Quantity.Should().Be(finalQuantity);
    }

    [Theory, AutoData]
    public void AddQuantity_ZeroQuantity_ShouldReturnDomainException(Item item)
    {
        Exception domainException = Record.Exception(() => item.AddQuantity(0));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductQuantityLessThanEqualToZero);
    }

    [Theory, AutoData]
    public void RemoveQuantity_ValidQuantity_ShouldRemoveQuantity(Item item, int quantity)
    {
        int finalQuantity = (item.Quantity + quantity) - quantity;
        item.AddQuantity(quantity);

        item.RemoveQuantity(quantity);

        item.Quantity.Should().Be(finalQuantity);
    }

    [Theory, AutoData]
    public void RemoveQuantity_ZeroQuantity_ShouldReturnDomainException(Item item)
    {
        Exception domainException = Record.Exception(() => item.RemoveQuantity(0));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductQuantityLessThanEqualToZero);
    }

    [Theory, AutoData]
    public void RemoveQuantity_QuantityGreaterThanAvailable_ShouldReturnDomainException(Item item)
    {
        int quantity = item.Quantity + 1;

        Exception domainException = Record.Exception(() => item.RemoveQuantity(quantity));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.QuantityGreaterThanAvailable);
    }
}
