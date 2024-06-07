using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Queries.GetCategoryById;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandlerTest
{
    [Theory, AutoCategoryData]
    public async Task Handle_ValidGetCategoryByIdQuery_ShouldInvokeReadRepositoryGetCategoryByIdAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        GetCategoryByIdQueryHandler sut,
        GetCategoryByIdQuery query)
    {
        await sut.Handle(query, CancellationToken.None);

        repositoryMock.Verify(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
