using AutoFixture.Xunit2;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandlerTest
{
    [Theory, AutoProductData]
    public async Task Handle_ValidGetProductByIdQuery_ShouldInvokeReadRepositoryGetProductByIdAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        GetProductByIdQueryHandler sut,
        GetProductByIdQuery query)
    {
        await sut.Handle(query, CancellationToken.None);

        repositoryMock.Verify(x => x.GetProductByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
