using AutoFixture.Xunit2;
using AutoMapper;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Events.CategoryAdded;

public sealed class CategoryAddedEventHandlerTest
{
    [Theory, AutoCategoryData]
    public async Task Handle_ValidCategoryAddedEvent_ShouldInvokeReadRepositoryAddCategoryAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        [Frozen] Mock<IMapper> mapperMock,
        CategoryAddedEventHandler sut,
        CategoryAddedEvent _event,
        Category category)
    {
        mapperMock
            .Setup(x => x.Map<Category>(It.IsAny<CategoryAddedEvent>()))
            .Returns(category);

        await sut.Handle(_event, CancellationToken.None);

        repositoryMock.Verify(x => x.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
