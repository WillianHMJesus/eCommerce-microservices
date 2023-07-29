using Bogus;
using EM.Carts.Application.UseCases.AddItem;
using System;

namespace EM.Carts.UnitTests.Fixtures.Application;

public sealed class AddItemRequestFixture
{
    public AddItemRequest GenerateValidAddItemRequest()
    {
        Faker<AddItemRequest> faker = new Faker<AddItemRequest>(locale: "en")
            .RuleFor(x => x.UserId, Guid.NewGuid())
            .RuleFor(x => x.ProductId, Guid.NewGuid())
            .RuleFor(x => x.ProductImage, f => f.Image.PicsumUrl())
            .RuleFor(x => x.ProductName, f => f.Commerce.ProductName())
            .RuleFor(x => x.Quantity, f => f.Random.Int(2, 15))
            .RuleFor(x => x.Value, f => f.Random.Decimal(1, 9999));

        return faker.Generate();
    }
}
