using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Events.ProductUpdated;

public sealed class ProductUpdatedEventHandlerTest
{
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly ProductUpdatedEventHandler _productUpdatedEventHandler;
    private readonly ProductUpdatedEvent _productUpdatedEvent;

    public ProductUpdatedEventHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = fixture.Freeze<Mock<IReadRepository>>();
        Product product = fixture.Create<Product>();

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Product>(It.IsAny<ProductUpdatedEvent>()))
            .Returns(product);

        _productUpdatedEventHandler = fixture.Create<ProductUpdatedEventHandler>();
        _productUpdatedEvent = fixture.Create<ProductUpdatedEvent>();
    }

    [Fact]
    public async Task Handle_ValidProductUpdatedEvent_ShouldInvokeReadRepositoryUpdateProductAsync()
    {
        await _productUpdatedEventHandler.Handle(_productUpdatedEvent, CancellationToken.None);

        _repositoryMock.Verify(x => x.UpdateProductAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()));
    }
}
