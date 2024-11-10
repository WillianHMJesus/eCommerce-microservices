using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands.DeleteCategory;
using EM.Catalog.Application.Categories.Events.CategoryDeleted;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.DeleteCategory;

public sealed class DeleteCategoryCommandHandlerTest
{
    [Theory, AutoCategoryData]
    public async Task Handle_ValidCommit_ShouldReturnWithSuccess(
        [Frozen] Mock<IWriteRepository> writeRepository,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        DeleteCategoryCommandHandler sut,
        DeleteCategoryCommand command)
    {
        Result result = await sut.Handle(command, CancellationToken.None);

        writeRepository.Verify(x => x.DeleteCategory(It.IsAny<Category>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish(It.IsAny<CategoryDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Theory, AutoCategoryData]
    public async Task Handle_InvalidCommit_ShouldReturnWithFailure(
        [Frozen] Mock<IWriteRepository> writeRepository,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        DeleteCategoryCommandHandler sut,
        DeleteCategoryCommand command)
    {
        unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Result result = await sut.Handle(command, CancellationToken.None);

        writeRepository.Verify(x => x.DeleteCategory(It.IsAny<Category>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish(It.IsAny<CategoryDeletedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Key == Key.CategoryAnErrorOccorred);
    }
}
