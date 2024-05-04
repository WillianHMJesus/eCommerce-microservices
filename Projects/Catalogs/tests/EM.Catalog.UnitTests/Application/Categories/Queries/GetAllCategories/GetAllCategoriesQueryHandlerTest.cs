using AutoFixture;
using AutoFixture.AutoMoq;
using EM.Catalog.Application.Categories.Queries.GetAllCategories;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Queries.GetAllCategories;

public sealed class GetAllCategoriesQueryHandlerTest
{
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly GetAllCategoriesQueryHandler _getAllCategoriesQueryHandler;
    private readonly GetAllCategoriesQuery _getAllCategoriesQuery;

    public GetAllCategoriesQueryHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = fixture.Freeze<Mock<IReadRepository>>();
        _getAllCategoriesQueryHandler = fixture.Create<GetAllCategoriesQueryHandler>();
        _getAllCategoriesQuery = fixture.Create<GetAllCategoriesQuery>();
    }

    [Fact]
    public async Task Handle_ValidGetAllCategoriesQuery_ShouldInvokeReadRepositoryGetAllCategoriesAsync()
    {
        await _getAllCategoriesQueryHandler.Handle(_getAllCategoriesQuery, CancellationToken.None);

        _repositoryMock.Verify(x => x.GetAllCategoriesAsync(It.IsAny<short>(), It.IsAny<short>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
