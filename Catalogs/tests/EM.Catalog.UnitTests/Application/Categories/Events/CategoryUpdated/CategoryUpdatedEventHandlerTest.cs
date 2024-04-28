using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Events.CategoryUpdated;

public sealed class CategoryUpdatedEventHandlerTest
{
    [Fact]
    public async void Handle_ValidCategoryUpdatedEvent_ShouldInvokeReadRepositoryUpdateCategoryAsync()
    {
        Mock<IReadRepository> readRepositoryMock = new();
        Mock<IMapper> mapperMock = new();
        Category category = new Fixture().Create<Category>();
        mapperMock.Setup(x => x.Map<Category>(It.IsAny<CategoryUpdatedEvent>())).Returns(category);
        CategoryUpdatedEventHandler categoryUpdatedEvent = new(readRepositoryMock.Object, mapperMock.Object);

        await categoryUpdatedEvent.Handle(new Fixture().Create<CategoryUpdatedEvent>(), It.IsAny<CancellationToken>());

        readRepositoryMock.Verify(x => x.UpdateCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
