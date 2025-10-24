using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Queries;
using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Application.Categories.Queries.GetCategoryById;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Queries;

public sealed class CategoryQueryHandlerTests
{
    [Theory, AutoCategoryData]
    [Trait("Test", "GetAllCategories:ValidGetAllCategoriesQuery")]
    public async Task GetAllCategories_ValidGetAllCategoriesQuery_ShouldInvokeReadRepositoryGetAllCategoriesAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        CategoryQueryHandler sut,
        GetAllCategoriesQuery query)
    {
        //Arrange & Act
        await sut.Handle(query, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.GetAllCategoriesAsync(It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "GetCategoryById:ValidGetCategoryByIdQuery")]
    public async Task GetCategoryById_ValidGetCategoryByIdQuery_ShouldInvokeReadRepositoryGetCategoryByIdAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        CategoryQueryHandler sut,
        GetCategoryByIdQuery query)
    {
        //Arrange & Act
        await sut.Handle(query, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
