using AutoFixture.Xunit2;
using EM.Catalog.Application.Products.Commands.UpdateProduct;
using EM.Catalog.Application.Products.Events.ProductUpdated;
using EM.Catalog.Domain.Entities;
using EM.Catalog.Domain.Interfaces;
using EM.Catalog.UnitTests.CustomAutoData;
using EM.Common.Core.ResourceManagers;
using FluentAssertions;
using MediatR;
using Moq;
using Xunit;

namespace EM.Catalog.UnitTests.Application.Products.Commands.UpdateProduct;

public sealed class UpdateProductCommandHandlerTest
{
    [Theory, AutoProductData]
    public async Task Handle_ValidCommit_ShouldReturnWithSuccess(
        [Frozen] Mock<IWriteRepository> writeRepository,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        UpdateProductCommandHandler sut,
        UpdateProductCommand command)
    {
        Result result = await sut.Handle(command, CancellationToken.None);

        writeRepository.Verify(x => x.UpdateProduct(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish(It.IsAny<ProductUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        result.Success.Should().BeTrue();
        result.Data.Should().BeNull();
    }

    [Theory, AutoProductData]
    public async Task Handle_InvalidCommit_ShouldReturnWithFailure(
        [Frozen] Mock<IWriteRepository> writeRepository,
        [Frozen] Mock<IUnitOfWork> unitOfWorkMock,
        [Frozen] Mock<IMediator> mediatorMock,
        UpdateProductCommandHandler sut,
        UpdateProductCommand command)
    {
        unitOfWorkMock
            .Setup(x => x.CommitAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        Result result = await sut.Handle(command, CancellationToken.None);

        writeRepository.Verify(x => x.UpdateProduct(It.IsAny<Product>()), Times.Once);
        unitOfWorkMock.Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(x => x.Publish(It.IsAny<ProductUpdatedEvent>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Success.Should().BeFalse();
        result.Errors.Should().Contain(x => x.Key == Key.ProductAnErrorOccorred);
    }
}
