using AutoFixture.Xunit2;
using EM.Catalog.Application.Products.Queries.SearchProducts;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Queries.SearchProducts;

public sealed class SearchQueryHandlerTest
{
    [Theory, AutoProductData]
    public async Task Handle_ValidSearchQuery_ShouldInvokeReadRepositorySearchProductsAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        SearchProductsQueryHandler sut,
        SearchProductsQuery query)
    {
        await sut.Handle(query, CancellationToken.None);

        repositoryMock.Verify(x => x.SearchProductsAsync(It.IsAny<string>(), It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
