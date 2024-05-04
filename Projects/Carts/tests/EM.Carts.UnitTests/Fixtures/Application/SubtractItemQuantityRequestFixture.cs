using Bogus;
using EM.Carts.Application.UseCases.SubtractItemQuantity;
using System;

namespace EM.Carts.UnitTests.Fixtures.Application;

public sealed class SubtractItemQuantityRequestFixture
{
    public SubtractItemQuantityRequest GenerateValidSubtractItemQuantityRequest()
    {
        Faker<SubtractItemQuantityRequest> Faker = new Faker<SubtractItemQuantityRequest>(locale: "en")
            .RuleFor(x => x.UserId, Guid.NewGuid())
            .RuleFor(x => x.ProductId, Guid.NewGuid())
            .RuleFor(x => x.Quantity, f => f.Random.Int(2, 15));

        return Faker.Generate();
    }
}
