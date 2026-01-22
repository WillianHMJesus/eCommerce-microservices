using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Bogus;
using EM.Catalog.Application.Products.Commands.AddProduct;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using Moq;
using WH.SimpleMapper;

namespace EM.Catalog.UnitTests.CustomAutoData;

public class AutoProductDataAttribute : AutoDataAttribute
{
    public AutoProductDataAttribute()
        : base(CreateFixture)
    { }

    private static IFixture CreateFixture()
    {
        IFixture fixture = new Fixture()
            .Customize(new AutoMoqCustomization { ConfigureMembers = true });

        Faker faker = new Faker();
        fixture.Register(() => faker);

        var product = fixture.Create<Product>();
        var category = fixture.Create<Category>();

        product.SetCategory(category);
        fixture.Register(() => product);

        fixture.Freeze<Mock<IMapper>>().Setup(x =>
            x.Map<AddProductCommand, Product>(It.IsAny<AddProductCommand>())).Returns(product);

        fixture.Freeze<Mock<IMapper>>().Setup(x =>
            x.Map<UpdateProductCommand, Product>(It.IsAny<UpdateProductCommand>())).Returns(product);

        return fixture;
    }
}
