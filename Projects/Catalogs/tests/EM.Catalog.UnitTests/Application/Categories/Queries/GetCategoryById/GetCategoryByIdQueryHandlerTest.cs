using AutoFixture;
using AutoFixture.AutoMoq;
using EM.Catalog.Application.Categories.Queries.GetCategoryById;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Queries.GetCategoryById;

public sealed class GetCategoryByIdQueryHandlerTest
{
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly GetCategoryByIdQueryHandler _getCategoryByIdQueryHandler;
    private readonly GetCategoryByIdQuery _getCategoryByIdQuery;

    public GetCategoryByIdQueryHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = fixture.Freeze<Mock<IReadRepository>>();
        _getCategoryByIdQueryHandler = fixture.Create<GetCategoryByIdQueryHandler>();
        _getCategoryByIdQuery = fixture.Create<GetCategoryByIdQuery>();
        Category? category = fixture.Create<Category?>();

        _repositoryMock
            .Setup(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(category));
    }

    [Fact]
    public async Task Handle_ValidGetCategoryByIdQuery_ShouldInvokeReadRepositoryGetCategoryByIdAsync()
    {
        await _getCategoryByIdQueryHandler.Handle(_getCategoryByIdQuery, CancellationToken.None);

        _repositoryMock.Verify(x => x.GetCategoryByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
