using Bogus;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.UnitTests.Fixtures;

public sealed class ProductFixture
{
    public Product GenerateProduct()
    {
        Faker<Product> productFaker = new Faker<Product>(locale: "en")
            .CustomInstantiator(x => new Product(
                x.Commerce.ProductName(),
                x.Commerce.ProductDescription(),
                x.Random.Decimal(1, 9999),
                x.Random.Short(1, 10),
                x.Image.PicsumUrl()));

        return productFaker.Generate();
    }

    public Product GenerateProductWithInvalidName()
    {
        Faker<Product> productFaker = new Faker<Product>(locale: "en")
            .CustomInstantiator(x => new Product(
                "",
                x.Commerce.ProductDescription(),
                x.Random.Decimal(1, 9999),
                x.Random.Short(1, 10),
                x.Image.PicsumUrl()));

        return productFaker.Generate();
    }

    public Product GenerateProductWithInvalidDescription()
    {
        Faker<Product> productFaker = new Faker<Product>(locale: "en")
            .CustomInstantiator(x => new Product(
                x.Commerce.ProductName(),
                "",
                x.Random.Decimal(1, 9999),
                x.Random.Short(1, 10),
                x.Image.PicsumUrl()));

        return productFaker.Generate();
    }

    public Product GenerateProductWithInvalidValue()
    {
        Faker<Product> productFaker = new Faker<Product>(locale: "en")
            .CustomInstantiator(x => new Product(
                x.Commerce.ProductName(),
                x.Commerce.ProductDescription(),
                0,
                x.Random.Short(1, 10),
                x.Image.PicsumUrl()));

        return productFaker.Generate();
    }

    public Product GenerateProductWithInvalidQuantity()
    {
        Faker<Product> productFaker = new Faker<Product>(locale: "en")
            .CustomInstantiator(x => new Product(
                x.Commerce.ProductName(),
                x.Commerce.ProductDescription(),
                x.Random.Decimal(1, 9999),
                0,
                x.Image.PicsumUrl()));

        return productFaker.Generate();
    }

    public Product GenerateProductWithInvalidImage()
    {
        Faker<Product> productFaker = new Faker<Product>(locale: "en")
            .CustomInstantiator(x => new Product(
                x.Commerce.ProductName(),
                x.Commerce.ProductDescription(),
                x.Random.Decimal(1, 9999),
                x.Random.Short(1, 10),
                ""));

        return productFaker.Generate();
    }
}
