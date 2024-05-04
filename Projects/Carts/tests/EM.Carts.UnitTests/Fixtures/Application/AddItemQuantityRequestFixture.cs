using Bogus;
using EM.Carts.Application.UseCases.AddItemQuantity;
using System;

namespace EM.Carts.UnitTests.Fixtures.Application;

public sealed class AddItemQuantityRequestFixture
{
    public AddItemQuantityRequest GenerateValidAddItemQuantityRequest()
    {
        Faker<AddItemQuantityRequest> Faker = new Faker<AddItemQuantityRequest>(locale: "en")
            .RuleFor(x => x.UserId, Guid.NewGuid())
            .RuleFor(x => x.ProductId, Guid.NewGuid())
            .RuleFor(x => x.Quantity, f => f.Random.Int(2, 15));

        return Faker.Generate();
    }
}
