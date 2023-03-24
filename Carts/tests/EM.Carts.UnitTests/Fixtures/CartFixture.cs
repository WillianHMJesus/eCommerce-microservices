using EM.Carts.Domain.Entities;
using System;

namespace EM.Carts.UnitTests.Fixtures;

public class CartFixture
{
    public Cart GenerateValidCart()
    {
        Guid userId = Guid.Parse("2e851a40-f717-4d72-bc3f-1fd3927c13f5");
        return new Cart(userId);
    }
}
