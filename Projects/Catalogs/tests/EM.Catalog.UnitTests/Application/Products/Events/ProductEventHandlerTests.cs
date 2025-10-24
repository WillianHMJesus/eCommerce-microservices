using AutoFixture.Xunit2;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.Application.Products;
using EM.Catalog.Application.Products.Events;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Application.Products.Events.ProductDeleted;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Events;

public sealed class ProductEventHandlerTests
{
    [Theory, AutoProductData]
    [Trait("Test", "ProductAdded:ValidProductAddedEvent")]
    public async Task ProductAdded_ValidProductAddedEvent_ShouldInvokeReadRepositoryAddProductAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        ProductEventHandler sut,
        ProductAddedEvent _event)
    {
        //Arrange & Act
        await sut.Handle(_event, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddAsync(It.IsAny<ProductDTO>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AutoProductData]
    [Trait("Test", "ProductDeleted:ValidProductDeletedEvent")]
    public async Task ProductDeleted_ValidProductDeletedEvent_ShouldInvokeReadRepositoryDeleteProductAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        ProductEventHandler sut,
        ProductDeletedEvent _event)
    {
        //Arrange & Act
        await sut.Handle(_event, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.DeleteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AutoProductData]
    [Trait("Test", "ProductUpdated:ValidProductUpdatedEvent")]
    public async Task ProductUpdated_ValidProductUpdatedEvent_ShouldInvokeReadRepositoryUpdateProductAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        ProductEventHandler sut,
        ProductUpdatedEvent _event)
    {
        //Arrange & Act
        await sut.Handle(_event, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<ProductDTO>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
