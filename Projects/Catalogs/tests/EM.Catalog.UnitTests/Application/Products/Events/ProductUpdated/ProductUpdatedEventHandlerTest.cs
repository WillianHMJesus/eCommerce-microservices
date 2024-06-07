using AutoFixture.Xunit2;
using AutoMapper;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Events.ProductUpdated;

public sealed class ProductUpdatedEventHandlerTest
{
    [Theory, AutoProductData]
    public async Task Handle_ValidProductUpdatedEvent_ShouldInvokeReadRepositoryUpdateProductAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        [Frozen] Mock<IMapper> mapperMock,
        ProductUpdatedEventHandler sut,
        ProductUpdatedEvent _event,
        Product product)
    {
        mapperMock
            .Setup(x => x.Map<Product>(It.IsAny<ProductUpdatedEvent>()))
            .Returns(product);

        await sut.Handle(_event, CancellationToken.None);

        repositoryMock.Verify(x => x.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
    }
}
