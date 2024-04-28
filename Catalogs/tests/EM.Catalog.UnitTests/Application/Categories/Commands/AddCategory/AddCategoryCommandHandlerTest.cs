using AutoFixture;
using AutoFixture.AutoMoq;
using AutoMapper;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Application.Results;
using EM.Catalog.Domain;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandHandlerTest
{
    private readonly IFixture _fixture;

    public AddCategoryCommandHandlerTest()
    {
        _fixture = new Fixture().Customize(new AutoMoqCustomization());
    }

    [Fact]
    public async void Handle_ValidAddCategoryCommand_ShouldInvokeMediatorPublish()
    {
        Category category = new Fixture().Create<Category>();
        Mock<IWriteRepository> repositoryMock = _fixture.Freeze<Mock<IWriteRepository>>();
        Mock<IMediator> mediatorMock = _fixture.Freeze<Mock<IMediator>>();

        _fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Category>(It.IsAny<AddCategoryCommand>()))
            .Returns(category);
        _fixture.Freeze<Mock<IUnitOfWork>>()
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(true));

        AddCategoryCommandHandler addCategoryHandler = _fixture.Create<AddCategoryCommandHandler>();
        Result result = await addCategoryHandler.Handle(new Fixture().Create<AddCategoryCommand>(), It.IsAny<CancellationToken>());

        repositoryMock.Verify(x => x.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish(It.IsAny<CategoryAddedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.True(result.Success);
        Assert.Equal(category.Id, result.Data);
    }

    [Fact]
    public async void Handle_ValidAddCategoryCommand_ShouldNotInvokeMediatorPublish()
    {
        Category category = new Fixture().Create<Category>();
        Mock<IWriteRepository> repositoryMock = _fixture.Freeze<Mock<IWriteRepository>>();
        Mock<IMediator> mediatorMock = _fixture.Freeze<Mock<IMediator>>();

        _fixture.Freeze<Mock<IMapper>>()
            .Setup(x => x.Map<Category>(It.IsAny<AddCategoryCommand>()))
            .Returns(category);
        _fixture.Freeze<Mock<IUnitOfWork>>()
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(false));

        AddCategoryCommandHandler addCategoryHandler = _fixture.Create<AddCategoryCommandHandler>();
        Result result = await addCategoryHandler.Handle(new Fixture().Create<AddCategoryCommand>(), It.IsAny<CancellationToken>());

        repositoryMock.Verify(x => x.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish(It.IsAny<CategoryAddedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        Assert.False(result.Success);
        Assert.Contains(result.Errors, x => x.Message == ErrorMessage.CategoryAnErrorOccorred);
    }
}
