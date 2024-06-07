using AutoFixture.Xunit2;
using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsQueryHandlerTest
{
    [Theory, AutoProductData]
    public async Task Handle_ValidGetAllProductsQuery_ShouldInvokeReadRepositoryGetAllProductsAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        GetAllProductsQueryHandler sut,
        GetAllProductsQuery query)
    {
        await sut.Handle(query, CancellationToken.None);

        repositoryMock.Verify(x => x.GetAllProductsAsync(It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
