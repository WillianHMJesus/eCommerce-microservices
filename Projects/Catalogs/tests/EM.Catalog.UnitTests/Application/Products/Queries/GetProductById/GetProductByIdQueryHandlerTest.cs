using AutoFixture;
using AutoFixture.AutoMoq;
using EM.Catalog.Application.Products.Queries.GetProductById;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Queries.GetProductById;

public sealed class GetProductByIdQueryHandlerTest
{
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly GetProductByIdQueryHandler _getProductByIdQueryHandler;
    private readonly GetProductByIdQuery _getProductByIdQuery;

    public GetProductByIdQueryHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = fixture.Freeze<Mock<IReadRepository>>();
        _getProductByIdQueryHandler = fixture.Create<GetProductByIdQueryHandler>();
        _getProductByIdQuery = fixture.Create<GetProductByIdQuery>();
        Product? product = fixture.Create<Product?>();

        _repositoryMock
            .Setup(x => x.GetProductByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(product));
    }

    [Fact]
    public async Task Handle_ValidGetProductByIdQuery_ShouldInvokeReadRepositoryGetProductByIdAsync()
    {
        await _getProductByIdQueryHandler.Handle(_getProductByIdQuery, CancellationToken.None);

        _repositoryMock.Verify(x => x.GetProductByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
