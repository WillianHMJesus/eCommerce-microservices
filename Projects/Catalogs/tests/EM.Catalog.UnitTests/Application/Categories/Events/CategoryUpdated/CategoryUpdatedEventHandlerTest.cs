using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Events.CategoryUpdated;

public sealed class CategoryUpdatedEventHandlerTest
{
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly CategoryUpdatedEventHandler _categoryUpdatedEventHandler;
    private readonly CategoryUpdatedEvent _categoryUpdatedEvent;

    public CategoryUpdatedEventHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = fixture.Freeze<Mock<IReadRepository>>();
        Category category = fixture.Create<Category>();

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Category>(It.IsAny<CategoryUpdatedEvent>()))
            .Returns(category);

        _categoryUpdatedEventHandler = fixture.Create<CategoryUpdatedEventHandler>();
        _categoryUpdatedEvent = fixture.Create<CategoryUpdatedEvent>();
    }

    [Fact]
    public async Task Handle_ValidCategoryUpdatedEvent_ShouldInvokeReadRepositoryUpdateCategoryAsync()
    {
        await _categoryUpdatedEventHandler.Handle(_categoryUpdatedEvent, CancellationToken.None);

        _repositoryMock.Verify(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
