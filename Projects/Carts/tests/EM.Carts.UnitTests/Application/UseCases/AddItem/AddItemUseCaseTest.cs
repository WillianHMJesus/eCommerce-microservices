using AutoFixture;
using AutoFixture.Xunit2;
using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;
using EM.Carts.UnitTests.Application.CustomAutoData;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application.UseCases.AddItem;

public sealed class AddItemUseCaseTest
{
    [Theory, AutoCartData]
    public async Task ExecuteAsync_NewCartAndNewItem_ShouldInvokeAddCartAsyncAndUpdateCartAsync(
        [Frozen] Mock<ICartRepository> repositoryMock,
        Mock<IPresenter> presenterMock,
        AddItemUseCase sut,
        AddItemRequest request)
    {
        sut.SetPresenter(presenterMock.Object);

        repositoryMock
            .Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(null as Cart);

        await sut.ExecuteAsync(request, CancellationToken.None);

        repositoryMock.Verify(x => x.AddCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        repositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        presenterMock.Verify(x => x.Success(null), Times.Once);
    }

    [Theory, AutoCartData]
    public async Task ExecuteAsync_ExistingCartAndNewItem_ShouldNotInvokeAddCartAsyncAndInvokeUpdateCartAsync(
        [Frozen] Mock<ICartRepository> repositoryMock,
        Mock<IPresenter> presenterMock,
        AddItemUseCase sut,
        AddItemRequest request)
    {
        sut.SetPresenter(presenterMock.Object);

        await sut.ExecuteAsync(request, CancellationToken.None);

        repositoryMock.Verify(x => x.AddCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Never);
        repositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        presenterMock.Verify(x => x.Success(null), Times.Once);
    }

    [Theory, AutoCartData]
    public async Task ExecuteAsync_ExistingCartAndItem_ShouldNotInvokeAddCartAsyncAndInvokeUpdateCartAsync(
        [Frozen] Mock<ICartRepository> repositoryMock,
        Mock<IPresenter> presenterMock,
        AddItemUseCase sut,
        Item item)
    {
        sut.SetPresenter(presenterMock.Object);

        AddItemRequest request = new Fixture().Build<AddItemRequest>()
            .With(x => x.ProductId, item.ProductId)
            .Create();

        await sut.ExecuteAsync(request, CancellationToken.None);

        repositoryMock.Verify(x => x.AddCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Never);
        repositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        presenterMock.Verify(x => x.Success(null), Times.Once);
    }
}
