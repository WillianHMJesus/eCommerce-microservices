using EM.Carts.Application.Interfaces;
using EM.Carts.Application.UseCases.DeleteItem;
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

public sealed class DeleteItemUseCaseTest
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<IPresenter> _presenterMock;
    private readonly DeleteItemUseCase _deleteItemUseCase;
    private readonly DeleteItemRequest _deleteItemRequest;

    public DeleteItemUseCaseTest()
    {
        _cartRepositoryMock = new();
        _presenterMock = new();
        _deleteItemUseCase = new(_cartRepositoryMock.Object);
        _deleteItemUseCase.SetPresenter(_presenterMock.Object);

        DeleteItemRequestFixture addItemQuantityRequestFixture = new();
        _deleteItemRequest = addItemQuantityRequestFixture.GenerateValidDeleteItemRequest();
    }

    [Fact]
    public async Task ExecuteAsync_ExitingCartAndItem_MustAddQuantity()
    {
        Cart cart = new(_deleteItemRequest.UserId);
        cart.AddItem(new ItemFixture().GenerateValidItem());
        _deleteItemRequest.ProductId = cart.Items.First().ProductId;
        _cartRepositoryMock.Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Cart?>(cart));

        await _deleteItemUseCase.ExecuteAsync(_deleteItemRequest);

        Assert.True(cart.Items.Count == 0);
        _cartRepositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Once);
        _presenterMock.Verify(x => x.Success(null), Times.Once);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_NotExitingCart_MustReturnBadRequest()
    {
        await _deleteItemUseCase.ExecuteAsync(_deleteItemRequest);

        _cartRepositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Never);
        _presenterMock.Verify(x => x.Success(null), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_NotExitingItem_MustReturnBadRequest()
    {
        Cart cart = new(_deleteItemRequest.UserId);
        _cartRepositoryMock.Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Cart?>(cart));

        await _deleteItemUseCase.ExecuteAsync(_deleteItemRequest);

        _cartRepositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Never);
        _presenterMock.Verify(x => x.Success(null), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }
}
