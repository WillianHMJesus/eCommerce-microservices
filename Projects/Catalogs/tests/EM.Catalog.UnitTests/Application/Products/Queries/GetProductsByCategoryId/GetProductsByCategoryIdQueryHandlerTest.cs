using AutoFixture.Xunit2;
using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Queries.GetProductsByCategoryId;

public sealed class GetProductsByCategoryIdQueryHandlerTest
{
    [Theory, AutoProductData]
    public async Task Handle_ValidGetProductsByCategoryIdQuery_ShouldInvokeReadRepositoryGetProductsByCategoryIdAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        GetProductsByCategoryIdQueryHandler sut,
        GetProductsByCategoryIdQuery query)
    {
        await sut.Handle(query, CancellationToken.None);

        repositoryMock.Verify(x => x.GetProductsByCategoryIdAsync(It.IsAny<Guid>(), It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
