using AutoFixture;
using AutoFixture.AutoMoq;
using EM.Catalog.Application.Products.Queries.GetAllProducts;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Queries.GetAllProducts;

public sealed class GetAllProductsQueryHandlerTest
{
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly GetAllProductsQueryHandler _getAllProductsQueryHandler;
    private readonly GetAllProductsQuery _getAllProductsQuery;

    public GetAllProductsQueryHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = fixture.Freeze<Mock<IReadRepository>>();
        _getAllProductsQueryHandler = fixture.Create<GetAllProductsQueryHandler>();
        _getAllProductsQuery = fixture.Create<GetAllProductsQuery>();
    }

    [Fact]
    public async Task Handle_ValidGetAllProductsQuery_ShouldInvokeReadRepositoryGetAllProductsAsync()
    {
        await _getAllProductsQueryHandler.Handle(_getAllProductsQuery, CancellationToken.None);

        _repositoryMock.Verify(x => x.GetAllProductsAsync(It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
