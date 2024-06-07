using AutoFixture.Xunit2;
using AutoMapper;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Events.CategoryUpdated;

public sealed class CategoryUpdatedEventHandlerTest
{
    [Theory, AutoCategoryData]
    public async Task Handle_ValidCategoryUpdatedEvent_ShouldInvokeReadRepositoryUpdateCategoryAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        [Frozen] Mock<IMapper> mapperMock,
        CategoryUpdatedEventHandler sut,
        CategoryUpdatedEvent _event,
        Category category)
    {
        mapperMock
            .Setup(x => x.Map<Category>(It.IsAny<CategoryUpdatedEvent>()))
            .Returns(category);

        await sut.Handle(_event, CancellationToken.None);

        repositoryMock.Verify(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
