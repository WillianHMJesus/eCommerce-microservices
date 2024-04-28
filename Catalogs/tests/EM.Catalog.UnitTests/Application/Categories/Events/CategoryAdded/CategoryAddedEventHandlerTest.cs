using AutoFixture;
using AutoMapper;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Events.CategoryAdded;

public sealed class CategoryAddedEventHandlerTest
{
    [Fact]
    public async void Handle_ValidCategoryAddedEvent_ShouldInvokeReadRepositoryAddCategoryAsync()
    {
        Mock<IReadRepository> readRepositoryMock = new();
        Mock<IMapper> mapperMock = new();
        Category category = new Fixture().Create<Category>();
        mapperMock.Setup(x => x.Map<Category>(It.IsAny<CategoryAddedEvent>())).Returns(category);
        CategoryAddedEventHandler categoryAddedEventHandler = new CategoryAddedEventHandler(readRepositoryMock.Object, mapperMock.Object);

        await categoryAddedEventHandler.Handle(new Fixture().Create<CategoryAddedEvent>(), It.IsAny<CancellationToken>());

        readRepositoryMock.Verify(x => x.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
