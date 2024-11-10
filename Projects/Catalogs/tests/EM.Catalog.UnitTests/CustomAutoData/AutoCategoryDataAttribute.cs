using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using AutoFixture;
using AutoMapper;
using EM.Catalog.Domain.Entities;
using Moq;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Common.Core.ResourceManagers;
using EM.Catalog.Application.Categories.Validations;
using EM.Catalog.Domain.Interfaces;

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

        Category category = fixture.Create<Category>();

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Category>(It.IsAny<AddCategoryCommand>()))
            .Returns(category);

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Category>(It.IsAny<UpdateCategoryCommand>()))
            .Returns(category);

        fixture.Freeze<Mock<ICategoryValidations>>()
            .Setup(x => x.ValidateCategoryIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        fixture.Freeze<Mock<ICategoryValidations>>()
            .Setup(x => x.ValidateDuplicityAsync(It.IsAny<AddCategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        fixture.Freeze<Mock<ICategoryValidations>>()
            .Setup(x => x.ValidateDuplicityAsync(It.IsAny<UpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        fixture.Freeze<Mock<IUnitOfWork>>()
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        IEnumerable<Error> errors = new List<Error> { new Error(Key.CategoryAnErrorOccorred, "") };
        fixture.Freeze<Mock<IResourceManager>>()
            .Setup(x => x.GetErrorsByKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.CreateResponseWithErrors(errors));

        fixture.Customize<Category>(x => x.FromFactory(() => category));

        return fixture;
    }
}
