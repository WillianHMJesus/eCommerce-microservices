using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Events.ProductAdded;

public sealed class ProductAddedEventHandlerTest
{
    [Fact]
    public async void Handle_ValidProductAddedEvent_ShouldInvokeReadRepositoryAddProductAsync()
    {
        Mock<IReadRepository> readRepositoryMock = new();
        Mock<IMapper> mapperMock = new();
        mapperMock.Setup(x => x.Map<Product>(It.IsAny<ProductAddedEvent>())).Returns(new Fixture().Create<Product>());
        ProductAddedEventHandler productAddedEventHandler = new(readRepositoryMock.Object, mapperMock.Object);

        await productAddedEventHandler.Handle(new Fixture().Create<ProductAddedEvent>(), It.IsAny<CancellationToken>());

        readRepositoryMock.Verify(x => x.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
    }
}
