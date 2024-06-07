using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandlerTest
{
    [Theory, AutoCategoryData]
    public async Task Handle_ValidGetAllCategoriesQuery_ShouldInvokeReadRepositoryGetAllCategoriesAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        GetAllCategoriesQueryHandler sut,
        GetAllCategoriesQuery query)
    {
        await sut.Handle(query, CancellationToken.None);

        repositoryMock.Verify(x => x.GetAllCategoriesAsync(It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
