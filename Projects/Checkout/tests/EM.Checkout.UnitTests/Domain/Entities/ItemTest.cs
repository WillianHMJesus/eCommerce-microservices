using AutoFixture;
using AutoFixture.Xunit2;
using EM.Checkout.Domain.Entities;
using EM.Common.Core.Domain;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using System;
using Xunit;

namespace EM.Checkout.UnitTests.Domain.Entities;

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
    public void Validate_EmptyItemProductId_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.ProductId, Guid.Empty)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductInvalidId);
    }

    [Fact]
    public void Validate_EmptyItemProductName_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.ProductName, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Validate_NullItemProductName_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.ProductName, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Validate_EmptyItemProductImage_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.ProductImage, "")
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Validate_NullItemProductImage_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.ProductImage, null as string)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductNameNullOrEmpty);
    }

    [Fact]
    public void Validate_ZeroItemValue_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.Value, 0)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductValueLessThanEqualToZero);
    }

    [Fact]
    public void Validate_ZeroItemQuantity_ShouldReturnDomainException()
    {
        Item item = _fixture.Build<Item>()
            .With(x => x.Quantity, 0)
            .Create();

        DomainException domainException = Assert.Throws<DomainException>(() => item.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.ProductQuantityLessThanEqualToZero);
    }
}
