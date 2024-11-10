using AutoFixture.Xunit2;
using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.UseCases.DeleteAllItems;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;
using EM.Carts.UnitTests.Application.CustomAutoData;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application.UseCases.DeleteAllItems;

public sealed class DeleteAllItemsUseCaseTest
{
    [Theory, AutoCartData]
    public async Task ExecuteAsync_ExistingCart_ShouldInvokeUpdateCartAsync(
        [Frozen] Mock<ICartRepository> repositoryMock,
        Mock<IPresenter> presenterMock,
        DeleteAllItemsUseCase sut,
        DeleteAllItemsRequest request)
    {
        sut.SetPresenter(presenterMock.Object);

        await sut.ExecuteAsync(request, CancellationToken.None);

        repositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        presenterMock.Verify(x => x.Success(null), Times.Once);
    }

    [Theory, AutoCartData]
    public async Task ExecuteAsync_CartNotFound_ShouldThrowArgumentNullException(
        [Frozen] Mock<ICartRepository> repositoryMock,
        DeleteAllItemsUseCase sut,
        DeleteAllItemsRequest request)
    {
        repositoryMock
            .Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Cart);

        Exception exception = await Record.ExceptionAsync(() => sut.ExecuteAsync(request, CancellationToken.None));

        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
