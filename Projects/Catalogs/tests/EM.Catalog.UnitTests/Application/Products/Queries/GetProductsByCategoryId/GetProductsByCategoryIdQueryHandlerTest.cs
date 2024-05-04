using AutoFixture;
using AutoFixture.AutoMoq;
using EM.Catalog.Application.Products.Queries.GetProductsByCategoryId;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Queries.GetProductsByCategoryId;

public sealed class GetProductsByCategoryIdQueryHandlerTest
{
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly GetProductsByCategoryIdQueryHandler _getProductsByCategoryIdQueryHandler;
    private readonly GetProductsByCategoryIdQuery _getProductsByCategoryIdQuery;

    public GetProductsByCategoryIdQueryHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = fixture.Freeze<Mock<IReadRepository>>();
        _getProductsByCategoryIdQueryHandler = fixture.Create<GetProductsByCategoryIdQueryHandler>();
        _getProductsByCategoryIdQuery = fixture.Create<GetProductsByCategoryIdQuery>();
    }

    [Fact]
    public async Task Handle_ValidGetProductsByCategoryIdQuery_ShouldInvokeReadRepositoryGetProductsByCategoryIdAsync()
    {
        await _getProductsByCategoryIdQueryHandler.Handle(_getProductsByCategoryIdQuery, CancellationToken.None);

        _repositoryMock.Verify(x => x.GetProductsByCategoryIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
