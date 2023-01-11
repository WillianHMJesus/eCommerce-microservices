using Bogus;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.UnitTests.Fixtures;

public class CategoryFixture
{
    public Category GenerateCategory()
    {
        Faker<Category> categoryFaker = new Faker<Category>(locale: "en")
            .CustomInstantiator(x => new Category(
                x.Random.Short(1, 10),
                x.Commerce.Categories(1)[0],
                x.Commerce.ProductDescription()));

        return categoryFaker.Generate();
    }

    public Category GenerateCategoryWithInvalidCode()
    {
        Faker<Category> categoryFaker = new Faker<Category>(locale: "en")
            .CustomInstantiator(x => new Category(
                0,
                x.Commerce.Categories(1)[0],
                x.Commerce.ProductDescription()));

        return categoryFaker.Generate();
    }

    public Category GenerateCategoryWithInvalidName()
    {
        Faker<Category> categoryFaker = new Faker<Category>(locale: "en")
            .CustomInstantiator(x => new Category(
                x.Random.Short(1, 10),
                "",
                x.Commerce.ProductDescription()));

        return categoryFaker.Generate();
    }

    public Category GenerateCategoryWithInvalidDescription()
    {
        Faker<Category> categoryFaker = new Faker<Category>(locale: "en")
            .CustomInstantiator(x => new Category(
                x.Random.Short(1, 10),
                x.Commerce.Categories(1)[0],
                ""));

        return categoryFaker.Generate();
    }
}
