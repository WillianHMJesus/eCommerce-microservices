using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using Bogus;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Domain.Entities;
using Moq;
using WH.SimpleMapper;

namespace EM.Catalog.UnitTests.CustomAutoData;

public class AutoCategoryDataAttribute : AutoDataAttribute
{
    public AutoCategoryDataAttribute()
        : base(CreateFixture)
    { }

    private static IFixture CreateFixture()
    {
        IFixture fixture = new Fixture()
            .Customize(new AutoMoqCustomization { ConfigureMembers = true });

        Faker faker = new Faker();
        fixture.Register(() => faker);

        var category = fixture.Create<Category>();
        fixture.Register(() => category);

        fixture.Freeze<Mock<IMapper>>().Setup(x =>
            x.Map<AddCategoryCommand, Category>(It.IsAny<AddCategoryCommand>())).Returns(category);

        fixture.Freeze<Mock<IMapper>>().Setup(x =>
            x.Map<UpdateCategoryCommand, Category>(It.IsAny<UpdateCategoryCommand>())).Returns(category);

        return fixture;
    }
}
