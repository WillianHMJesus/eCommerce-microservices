using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Events.CategoryDeleted;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Events.CategoryDeleted;

public sealed class CategoryDeletedEventHandlerTest
{
    [Theory, AutoCategoryData]
    public async Task Handle_ValidCategoryDeletedEvent_ShouldInvokeReadRepositoryDeleteCategoryAsync(
        [Frozen] Mock<IReadRepository> repositoryMock,
        CategoryDeletedEventHandler sut,
        CategoryDeletedEvent _event)
    {
        await sut.Handle(_event, CancellationToken.None);

        repositoryMock.Verify(x => x.DeleteCategoryAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
