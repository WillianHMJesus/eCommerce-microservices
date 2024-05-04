using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using EM.Catalog.Application.Products.Events.ProductAdded;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Events.ProductAdded;

public sealed class ProductAddedEventHandlerTest
{
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly ProductAddedEventHandler _productAddedEventHandler;
    private readonly ProductAddedEvent _productAddedEvent;

    public ProductAddedEventHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = fixture.Freeze<Mock<IReadRepository>>();
        Product product = fixture.Create<Product>();

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Product>(It.IsAny<ProductAddedEvent>()))
            .Returns(product);

        _productAddedEventHandler = fixture.Create<ProductAddedEventHandler>();
        _productAddedEvent = fixture.Create<ProductAddedEvent>();
    }

    [Fact]
    public async Task Handle_ValidProductAddedEvent_ShouldInvokeReadRepositoryAddProductAsync()
    {
        await _productAddedEventHandler.Handle(_productAddedEvent, CancellationToken.None);

        _repositoryMock.Verify(x => x.AddProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
    }
}
