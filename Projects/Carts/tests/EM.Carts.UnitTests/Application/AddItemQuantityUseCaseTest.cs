using EM.Carts.Application.Interfaces;
using EM.Carts.Application.UseCases.AddItemQuantity;
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

public sealed class AddItemQuantityUseCaseTest
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<IPresenter> _presenterMock;
    private readonly AddItemQuantityUseCase _addItemQuantityUseCase;
    private readonly AddItemQuantityRequest _addItemQuantityRequest;

    public AddItemQuantityUseCaseTest()
    {
        _cartRepositoryMock = new();
        _presenterMock = new();
        _addItemQuantityUseCase = new(_cartRepositoryMock.Object);
        _addItemQuantityUseCase.SetPresenter(_presenterMock.Object);

        AddItemQuantityRequestFixture addItemQuantityRequestFixture = new();
        _addItemQuantityRequest = addItemQuantityRequestFixture.GenerateValidAddItemQuantityRequest();
    }

    [Fact]
    public async Task ExecuteAsync_ExitingCartAndItem_MustAddQuantity()
    {
        Cart cart = new(_addItemQuantityRequest.UserId);
        cart.AddItem(new ItemFixture().GenerateValidItem());
        _addItemQuantityRequest.ProductId = cart.Items.First().ProductId;
        _cartRepositoryMock.Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Cart?>(cart));
        int itemQuantity = cart.Items.First().Quantity;

        await _addItemQuantityUseCase.ExecuteAsync(_addItemQuantityRequest);

        Assert.Equal(itemQuantity + _addItemQuantityRequest.Quantity, cart.Items.First().Quantity);
        _cartRepositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Once);
        _presenterMock.Verify(x => x.Success(null), Times.Once);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_NotExitingCart_MustReturnBadRequest()
    {
        await _addItemQuantityUseCase.ExecuteAsync(_addItemQuantityRequest);

        _cartRepositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Never);
        _presenterMock.Verify(x => x.Success(null), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_NotExitingItem_MustReturnBadRequest()
    {
        Cart cart = new(_addItemQuantityRequest.UserId);
        _cartRepositoryMock.Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Cart?>(cart));

        await _addItemQuantityUseCase.ExecuteAsync(_addItemQuantityRequest);

        _cartRepositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Never);
        _presenterMock.Verify(x => x.Success(null), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }
}
