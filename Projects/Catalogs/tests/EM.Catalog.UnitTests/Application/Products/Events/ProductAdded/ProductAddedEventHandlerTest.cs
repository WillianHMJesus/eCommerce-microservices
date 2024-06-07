using AutoFixture.Xunit2;
using AutoMapper;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Events.ProductAdded;

public sealed class ProductAddedEventHandlerTest
{
    [Theory, AutoProductData]
    public async Task Handle_ValidProductAddedEvent_ShouldInvokeReadRepositoryAddProductAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        [Frozen] Mock<IMapper> mapperMock,
        ProductAddedEventHandler sut,
        ProductAddedEvent _event,
        Product product)
    {
        mapperMock
            .Setup(x => x.Map<Product>(It.IsAny<ProductAddedEvent>()))
            .Returns(product);

        await sut.Handle(_event, CancellationToken.None);

        repositoryMock.Verify(x => x.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
    }
}
