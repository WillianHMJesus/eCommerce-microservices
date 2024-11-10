using AutoFixture.Xunit2;
using AutoFixture;
using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.UseCases.RemoveItemQuantity;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;
using EM.Carts.UnitTests.Application.CustomAutoData;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Collections.Generic;

namespace EM.Carts.UnitTests.Application.UseCases.RemoveItemQuantity;

public sealed class RemoveItemQuantityUseCaseTest
{
    [Theory, AutoCartData]
    public async Task ExecuteAsync_ExistingCartAndItem_ShouldInvokeUpdateCartAsync(
        [Frozen] Mock<ICartRepository> repositoryMock,
        Mock<IPresenter> presenterMock,
        RemoveItemQuantityUseCase sut,
        Item item)
    {
        sut.SetPresenter(presenterMock.Object);
        Cart cart = new(Guid.NewGuid());
        cart.AddItem(item);

        RemoveItemQuantityRequest request = new Fixture().Build<RemoveItemQuantityRequest>()
            .With(x => x.ProductId, item.ProductId)
            .With(x => x.Quantity, item.Quantity - 1)
            .Create();

        repositoryMock
            .Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        await sut.ExecuteAsync(request, CancellationToken.None);

        repositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        presenterMock.Verify(x => x.Success(null), Times.Once);
    }

    [Theory, AutoCartData]
    public async Task ExecuteAsync_RemoveItem_ShouldInvokeUpdateCartAsync(
        [Frozen] Mock<ICartRepository> repositoryMock,
        Mock<IPresenter> presenterMock,
        RemoveItemQuantityUseCase sut,
        Item item)
    {
        sut.SetPresenter(presenterMock.Object);
        Cart cart = new(Guid.NewGuid());
        cart.AddItem(item);

        RemoveItemQuantityRequest request = new Fixture().Build<RemoveItemQuantityRequest>()
            .With(x => x.ProductId, item.ProductId)
            .With(x => x.Quantity, item.Quantity)
            .Create();

        repositoryMock
            .Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(cart);

        await sut.ExecuteAsync(request, CancellationToken.None);

        repositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        presenterMock.Verify(x => x.Success(null), Times.Once);
    }

    [Theory, AutoCartData]
    public async Task ExecuteAsync_CartNotFound_ShouldThrownArgumentNullException(
        [Frozen] Mock<ICartRepository> repositoryMock,
        RemoveItemQuantityUseCase sut,
        RemoveItemQuantityRequest request)
    {
        repositoryMock
            .Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Cart);

        Exception exception = await Record.ExceptionAsync(() => sut.ExecuteAsync(request, CancellationToken.None));

        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentNullException>();
    }

    [Theory, AutoCartData]
    public async Task ExecuteAsync_ItemNotFound_ShouldThrownArgumentNullException(
        [Frozen] Mock<ICartRepository> repositoryMock,
        RemoveItemQuantityUseCase sut,
        RemoveItemQuantityRequest request)
    {
        repositoryMock
            .Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Cart(Guid.NewGuid()));

        Exception exception = await Record.ExceptionAsync(() => sut.ExecuteAsync(request, CancellationToken.None));

        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
