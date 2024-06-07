using AutoFixture.Xunit2;
using EM.Catalog.Application.Categories.Commands.UpdateCategory;
using EM.Catalog.Application.Categories.Events.CategoryUpdated;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Categories.Commands.UpdateCategory;

public sealed class UpdateCategoryCommandHandlerTest
{
    [Theory, AutoCategoryData]
    public async Task Handle_ValidCommit_MustReturnWithSuccess(
        [Frozen] Mock<IWriteRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        UpdateCategoryCommandHandler sut,
        UpdateCategoryCommand command)
    {
        unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        Result result = await sut.Handle(command, CancellationToken.None);

        repositoryMock.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()));
        mediatorMock.Verify(x => x.Publish(It.IsAny<CategoryUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Theory, AutoCategoryData]
    public async Task Handle_InvalidCommit_ShouldThrowDomainException(
        [Frozen] Mock<IWriteRepository> repositoryMock,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        UpdateCategoryCommandHandler sut,
        UpdateCategoryCommand command)
    {
        unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Result result = await sut.Handle(command, CancellationToken.None);

        repositoryMock.Verify(x => x.UpdateCategory(It.IsAny<Category>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish(It.IsAny<CategoryUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Key == Key.CategoryAnErrorOccorred);
    }
}
