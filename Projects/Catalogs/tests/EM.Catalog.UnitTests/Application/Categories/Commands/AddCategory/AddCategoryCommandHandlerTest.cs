using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands.AddCategory;
using EM.Catalog.Application.Categories.Events.CategoryAdded;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.AddCategory;

public sealed class AddCategoryCommandHandlerTest
{
    [Theory, AutoCategoryData]
    public async Task Handle_ValidCommit_ShouldReturnWithSuccess(
        [Frozen] Mock<IWriteRepository> writeRepository,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        AddCategoryCommandHandler sut,
        AddCategoryCommand command,
        Category category)
    {
        Result result = await sut.Handle(command, CancellationToken.None);

        writeRepository.Verify(x => x.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish(It.IsAny<CategoryAddedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().Be(category.Id);
    }

    [Theory, AutoCategoryData]
    public async Task Handle_InvalidCommit_ShouldReturnWithFailure(
        [Frozen] Mock<IWriteRepository> writeRepository,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        AddCategoryCommandHandler sut,
        AddCategoryCommand command)
    {
        unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Result result = await sut.Handle(command, CancellationToken.None);

        writeRepository.Verify(x => x.AddCategoryAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish(It.IsAny<CategoryAddedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Key == Key.CategoryAnErrorOccorred);
    }
}
