using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories;
using EM.Catalog.Application.Categories.Events;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Categories.Events.CategoryDeleted;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Application.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Events;

public sealed class CategoryEventHandlerTests
{
    [Theory, AutoCategoryData]
    [Trait("Test", "CategoryAdded:ValidCategoryAddedEvent")]
    public async Task CategoryAdded_ValidCategoryAddedEvent_ShouldInvokeReadRepositoryAddCategoryAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        CategoryEventHandler sut,
        CategoryAddedEvent _event)
    {
        //Arrange & Act
        await sut.Handle(_event, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.AddCategoryAsync(It.IsAny<CategoryDTO>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "CategoryDeleted:ValidCategoryDeletedEvent")]
    public async Task CategoryDeleted_ValidCategoryDeletedEvent_ShouldInvokeReadRepositoryDeleteCategoryAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        CategoryEventHandler sut,
        CategoryDeletedEvent _event)
    {
        //Arrange & Act
        await sut.Handle(_event, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.DeleteCategoryAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Theory, AutoCategoryData]
    [Trait("Test", "CategoryUpdated:ValidCategoryUpdatedEvent")]
    public async Task CategoryUpdated_ValidCategoryUpdatedEvent_ShouldInvokeReadRepositoryUpdateCategoryAsync(
        [Frozen] Mock<IProductReadRepository> repositoryMock,
        CategoryEventHandler sut,
        CategoryUpdatedEvent _event)
    {
        //Arrange & Act
        await sut.Handle(_event, CancellationToken.None);

        //Assert
        repositoryMock.Verify(x => x.UpdateCategoryAsync(It.IsAny<CategoryDTO>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
