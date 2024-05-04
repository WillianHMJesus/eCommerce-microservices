using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Events.CategoryAdded;

public sealed class CategoryAddedEventHandlerTest
{
    private readonly Mock<IReadRepository> _repositoryMock;
    private readonly CategoryAddedEventHandler _categoryAddedEventHandler;
    private readonly CategoryAddedEvent _categoryAddedEvent;

    public CategoryAddedEventHandlerTest()
    {
        IFixture fixture = new Fixture().Customize(new AutoMoqCustomization());
        _repositoryMock = fixture.Freeze<Mock<IReadRepository>>();
        Category category = fixture.Create<Category>();

        fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Category>(It.IsAny<CategoryAddedEvent>()))
            .Returns(category);

        _categoryAddedEventHandler = fixture.Create<CategoryAddedEventHandler>();
        _categoryAddedEvent = fixture.Create<CategoryAddedEvent>();
        
    }

    [Fact]
    public async Task Handle_ValidCategoryAddedEvent_ShouldInvokeReadRepositoryAddCategoryAsync()
    {
        await _categoryAddedEventHandler.Handle(_categoryAddedEvent, CancellationToken.None);

        _repositoryMock.Verify(x => x.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
