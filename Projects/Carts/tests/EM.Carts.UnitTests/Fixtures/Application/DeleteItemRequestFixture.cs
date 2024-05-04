using Bogus;
using EM.Carts.Application.UseCases.DeleteItem;
using System;

namespace EM.Carts.UnitTests.Fixtures.Application;

public sealed class DeleteItemRequestFixture
{
    public DeleteItemRequest GenerateValidDeleteItemRequest()
    {
        Faker<DeleteItemRequest> Faker = new Faker<DeleteItemRequest>(locale: "en")
            .RuleFor(x => x.UserId, Guid.NewGuid())
            .RuleFor(x => x.ProductId, Guid.NewGuid());

        return Faker.Generate();
    }
}
