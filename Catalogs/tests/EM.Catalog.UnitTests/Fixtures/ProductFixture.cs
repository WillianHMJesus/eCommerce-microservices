using Bogus;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.UnitTests.Fixtures;

internal class ProductFixture
{
    public Product GenerateProduct()
    {
        var productFaker = new Faker<Product>(locale: "en")
            .CustomInstantiator(x => new Product(
                x.Commerce.ProductName(),
                x.Commerce.ProductDescription(),
                x.Random.Decimal(1, 9999),
                x.Random.Int(1, 10),
                x.Image.PicsumUrl()));

        return productFaker.Generate();
    }

    public Product GenerateProductWithInvalidName()
    {
        var productFaker = new Faker<Product>(locale: "en")
            .CustomInstantiator(x => new Product(
                "",
                x.Commerce.ProductDescription(),
                x.Random.Decimal(1, 9999),
                x.Random.Int(1, 10),
                x.Image.PicsumUrl()));

        return productFaker.Generate();
    }

    public Product GenerateProductWithInvalidDescription()
    {
        var productFaker = new Faker<Product>(locale: "en")
            .CustomInstantiator(x => new Product(
                x.Commerce.ProductName(),
                "",
                x.Random.Decimal(1, 9999),
                x.Random.Int(1, 10),
                x.Image.PicsumUrl()));

        return productFaker.Generate();
    }

    public Product GenerateProductWithInvalidValue()
    {
        var productFaker = new Faker<Product>(locale: "en")
            .CustomInstantiator(x => new Product(
                x.Commerce.ProductName(),
                x.Commerce.ProductDescription(),
                0,
                x.Random.Int(1, 10),
                x.Image.PicsumUrl()));

        return productFaker.Generate();
    }

    public Product GenerateProductWithInvalidQuantity()
    {
        var productFaker = new Faker<Product>(locale: "en")
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
        var productFaker = new Faker<Product>(locale: "en")
            .CustomInstantiator(x => new Product(
                x.Commerce.ProductName(),
                x.Commerce.ProductDescription(),
                x.Random.Decimal(1, 9999),
                x.Random.Int(1, 10),
                ""));

        return productFaker.Generate();
    }
}
