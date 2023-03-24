using EM.Carts.Domain;
using EM.Carts.Domain.Entities;
using EM.Carts.UnitTests.Fixtures;
using System;
using System.Linq;
using Xunit;

namespace EM.Carts.UnitTests.Domain;

public class CartTest
{
    private readonly CartFixture _cartFixture;
    private readonly ItemFixture _itemFixture;

    public CartTest()
    {
        _cartFixture = new CartFixture();
        _itemFixture = new ItemFixture();
    }

    [Fact]
    public void Validate_ValidCart_MustNotReturnDomainException()
    {
        Exception domainException = Record.Exception(()
            => _cartFixture.GenerateValidCart());

        Assert.Null(domainException);
    }

    [Fact]
    public void Validate_InvalidUserId_MustReturnDomainException()
    {
        Exception domainException = Record.Exception(()
            => new Cart(Guid.Empty));

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.UserIdInvalid, domainException.Message);
    }

    [Fact]
    public void AddItem_ValidItem_MustAddCartItem()
    {
        Cart cart = _cartFixture.GenerateValidCart();

        Exception domainException = Record.Exception(() 
            => cart.AddItem(_itemFixture.GenerateValidItem()));

        Assert.True(cart.Items.Any());
        Assert.Null(domainException);
    }

    [Fact]
    public void AddItem_NullItem_MustReturnDomainException()
    {
        Cart cart = new Cart(Guid.NewGuid());

        Exception domainException = Record.Exception(()
            => cart.AddItem(null));

        Assert.NotNull(domainException);
        Assert.Equal(ErrorMessage.CartItemNull, domainException.Message);
    }
}
