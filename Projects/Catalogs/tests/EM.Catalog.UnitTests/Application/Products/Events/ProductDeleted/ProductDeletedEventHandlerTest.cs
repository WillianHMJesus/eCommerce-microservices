using AutoFixture.Xunit2;
using EM.Catalog.Application.Products.Events.ProductDeleted;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Events.ProductDeleted;

public sealed class ProductDeletedEventHandlerTest
{
    [Theory, AutoProductData]
    public async Task Handle_ValidProductDeletedEvent_ShouldInvokeReadRepositoryDeleteProductAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        ProductDeletedEventHandler sut,
        ProductDeletedEvent _event)
    {
        await sut.Handle(_event, CancellationToken.None);

        repositoryMock.Verify(x => x.DeleteProductAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
