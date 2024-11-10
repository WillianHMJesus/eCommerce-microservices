using AutoFixture;
using AutoFixture.Xunit2;
using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.UseCases.AddItemQuantity;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;
using EM.Carts.UnitTests.Application.CustomAutoData;
using FluentAssertions;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application.UseCases.AddItemQuantity;

public sealed class AddItemQuantityUseCaseTest
{
    [Theory, AutoCartData]
    public async Task ExecuteAsync_ExistingCartAndItem_ShouldInvokeUpdateCartAsync(
        [Frozen] Mock<ICartRepository> repositoryMock,
        [Frozen] Mock<IPresenter> presenterMock,
        AddItemQuantityUseCase sut,
        Item item)
    {
        sut.SetPresenter(presenterMock.Object);

        AddItemQuantityRequest request = new Fixture().Build<AddItemQuantityRequest>()
            .With(x => x.ProductId, item.ProductId)
            .Create();

        await sut.ExecuteAsync(request, CancellationToken.None);

        repositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>(), It.IsAny<CancellationToken>()), Times.Once);
        presenterMock.Verify(x => x.Success(null), Times.Once);
    }

    [Theory, AutoCartData]
    public async Task ExecuteAsync_CartNotFound_ShouldThrownArgumentNullException(
        [Frozen] Mock<ICartRepository> repositoryMock,
        AddItemQuantityUseCase sut,
        AddItemQuantityRequest request)
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
        AddItemQuantityUseCase sut,
        AddItemQuantityRequest request)
    {
        repositoryMock
            .Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Cart(Guid.NewGuid()));

        Exception exception = await Record.ExceptionAsync(() => sut.ExecuteAsync(request, CancellationToken.None));

        exception.Should().NotBeNull();
        exception.Should().BeOfType<ArgumentNullException>();
    }
}
