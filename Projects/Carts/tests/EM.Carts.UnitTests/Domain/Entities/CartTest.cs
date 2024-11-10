using AutoFixture;
using AutoFixture.Xunit2;
using EM.Carts.Domain.Entities;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace EM.Carts.UnitTests.Domain.Entities;

public sealed class CartTest
{
    private readonly Fixture _fixture;

    public CartTest() => _fixture = new();

    [Theory, AutoData]
    public void Validate_ValidCart_ShouldNotReturnDomainException(Cart cart)
    {
        Exception domainException = Record.Exception(() => cart.Validate());

        domainException.Should().BeNull();
    }

    [Fact]
    public void Validate_DefaultUserId_ShouldReturnDomainException()
    {
        Cart cart = _fixture.Build<Cart>()
            .With(x => x.UserId, Guid.Empty)
            .Create();

        Exception domainException = Record.Exception(() => cart.Validate());

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.UserIdInvalid);
    }

    [Theory, AutoData]
    public void AddItem_ValidItem_ShouldAddItem(Cart cart, Item item)
    {
        cart.AddItem(item);

        cart.Items.Should().Contain(x => x.Id == item.Id);
    }

    [Theory, AutoData]
    public void AddItem_NullItem_ShouldReturnDomainException(Cart cart)
    {
        Exception domainException = Record.Exception(() => cart.AddItem(null));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.CartItemNull);
    }

    [Theory, AutoData]
    public void RemoveItem_ValidItem_ShouldRemoveItem(Cart cart, Item item)
    {
        cart.RemoveAllItems();
        cart.AddItem(item);

        cart.RemoveItem(item);

        cart.Items.Should().BeEmpty();
    }

    [Theory, AutoData]
    public void RemoveItem_NullItem_ShouldReturnDomainException(Cart cart)
    {
        Exception domainException = Record.Exception(() => cart.RemoveItem(null));

        domainException.Should().NotBeNull();
        domainException.Message.Should().Be(Key.CartItemNull);
    }

    [Theory, AutoData]
    public void RemoveAllItems_ValidItems_ShouldRemoveAllItems(Cart cart, Item item)
    {
        cart.RemoveAllItems();
        cart.AddItem(item);

        cart.RemoveAllItems();

        cart.Items.Should().BeEmpty();
    }
}
