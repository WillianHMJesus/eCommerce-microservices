using EM.Carts.Application.Interfaces;
using EM.Carts.Application.UseCases.SubtractItemQuantity;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;
using EM.Carts.UnitTests.Fixtures.Application;
using EM.Carts.UnitTests.Fixtures.Domain;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application;

public sealed class SubtractItemQuantityUseCaseTest
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<IPresenter> _presenterMock;
    private readonly SubtractItemQuantityUseCase _subtractItemQuantityUseCase;
    private readonly SubtractItemQuantityRequest _subtractItemQuantityRequest;

    public SubtractItemQuantityUseCaseTest()
    {
        _cartRepositoryMock = new();
        _presenterMock = new();
        _subtractItemQuantityUseCase = new(_cartRepositoryMock.Object);
        _subtractItemQuantityUseCase.SetPresenter(_presenterMock.Object);

        SubtractItemQuantityRequestFixture subtractItemQuantityRequestFixture = new();
        _subtractItemQuantityRequest = subtractItemQuantityRequestFixture.GenerateValidSubtractItemQuantityRequest();
    }

    [Fact]
    public async Task ExecuteAsync_ExitingCartAndItem_MustSubtractQuantity()
    {
        Cart cart = new(_subtractItemQuantityRequest.UserId);
        cart.AddItem(new ItemFixture().GenerateValidItem());
        _subtractItemQuantityRequest.ProductId = cart.Items.First().ProductId;
        _subtractItemQuantityRequest.Quantity = cart.Items.First().Quantity - 1;
        _cartRepositoryMock.Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Cart?>(cart));
        int itemQuantity = cart.Items.First().Quantity;

        await _subtractItemQuantityUseCase.ExecuteAsync(_subtractItemQuantityRequest);

        Assert.Equal(itemQuantity - _subtractItemQuantityRequest.Quantity, cart.Items.First().Quantity);
        _cartRepositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Once);
        _presenterMock.Verify(x => x.Success(null), Times.Once);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_NotExitingCart_MustReturnBadRequest()
    {
        await _subtractItemQuantityUseCase.ExecuteAsync(_subtractItemQuantityRequest);

        _cartRepositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Never);
        _presenterMock.Verify(x => x.Success(null), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_NotExitingItem_MustReturnBadRequest()
    {
        Cart cart = new(_subtractItemQuantityRequest.UserId);
        _cartRepositoryMock.Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Cart?>(cart));

        await _subtractItemQuantityUseCase.ExecuteAsync(_subtractItemQuantityRequest);

        _cartRepositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Never);
        _presenterMock.Verify(x => x.Success(null), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }
}
