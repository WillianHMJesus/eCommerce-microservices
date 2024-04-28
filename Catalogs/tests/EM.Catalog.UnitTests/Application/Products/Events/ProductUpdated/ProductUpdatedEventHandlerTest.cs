using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Events.ProductUpdated;

public sealed class ProductUpdatedEventHandlerTest
{
    [Fact]
    public async void Handle_ValidProductUpdatedEvent_ShouldInvokeReadRepositoryUpdateProductAsync()
    {
        Mock<IReadRepository> readRepositoryMock = new();
        Mock<IMapper> mapperMock = new();
        mapperMock.Setup(x => x.Map<Product>(It.IsAny<ProductUpdatedEvent>())).Returns(new Fixture().Create<Product>());
        ProductUpdatedEventHandler productUpdatedEventHandler = new(readRepositoryMock.Object, mapperMock.Object);

        await productUpdatedEventHandler.Handle(new Fixture().Create<ProductUpdatedEvent>(), It.IsAny<CancellationToken>());

        readRepositoryMock.Verify(x => x.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
    }
}
