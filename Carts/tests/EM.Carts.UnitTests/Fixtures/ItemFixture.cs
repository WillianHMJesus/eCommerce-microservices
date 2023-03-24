using Bogus;
using EM.Carts.Domain.Entities;
using System;

namespace EM.Carts.UnitTests.Fixtures;

public class ItemFixture
{
    public Item GenerateValidItem()
    {
        Faker<Item> itemFaker = new Faker<Item>(locale: "en")
            .CustomInstantiator(x => new Item(
                Guid.NewGuid(),
                x.Commerce.ProductName(),
                x.Image.PicsumUrl(),
                x.Random.Decimal(1, 9999),
                x.Random.Int(2, 15)));

        return itemFaker.Generate();
    }

    public Item GenerateItemWithInvalidProductId()
    {
        Faker<Item> itemFaker = new Faker<Item>(locale: "en")
            .CustomInstantiator(x => new Item(
                Guid.Empty,
                x.Commerce.ProductName(),
                x.Image.PicsumUrl(),
                x.Random.Decimal(1, 9999),
                x.Random.Int(1, 15)));

        return itemFaker.Generate();
    }

    public Item GenerateItemWithoutProductName()
    {
        Faker<Item> itemFaker = new Faker<Item>(locale: "en")
            .CustomInstantiator(x => new Item(
                Guid.NewGuid(),
                "",
                x.Image.PicsumUrl(),
                x.Random.Decimal(1, 9999),
                x.Random.Int(1, 15)));

        return itemFaker.Generate();
    }

    public Item GenerateItemWithoutProductImage()
    {
        Faker<Item> itemFaker = new Faker<Item>(locale: "en")
            .CustomInstantiator(x => new Item(
                Guid.NewGuid(),
                x.Commerce.ProductName(),
                "",
                x.Random.Decimal(1, 9999),
                x.Random.Int(1, 15)));

        return itemFaker.Generate();
    }

    public Item GenerateItemWithInvalidValue()
    {
        Faker<Item> itemFaker = new Faker<Item>(locale: "en")
            .CustomInstantiator(x => new Item(
                Guid.NewGuid(),
                x.Commerce.ProductName(),
                x.Image.PicsumUrl(),
                0,
                x.Random.Int(1, 15)));

        return itemFaker.Generate();
    }

    public Item GenerateItemWithInvalidQuantity()
    {
        Faker<Item> itemFaker = new Faker<Item>(locale: "en")
            .CustomInstantiator(x => new Item(
                Guid.NewGuid(),
                x.Commerce.ProductName(),
                x.Image.PicsumUrl(),
                x.Random.Decimal(1, 9999),
                0));

        return itemFaker.Generate();
    }
}
