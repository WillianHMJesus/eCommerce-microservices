using EM.Carts.Application.Interfaces;
using EM.Carts.Application.UseCases.DeleteAllItems;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;
using EM.Carts.UnitTests.Fixtures.Domain;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application;

public sealed class DeleteAllItemsUseCaseTest
{
    private readonly Mock<ICartRepository> _cartRepositoryMock;
    private readonly Mock<IPresenter> _presenterMock;
    private readonly DeleteAllItemsUseCase _deleteAllItemsUseCase;

    public DeleteAllItemsUseCaseTest()
    {
        _cartRepositoryMock = new();
        _presenterMock = new();
        _deleteAllItemsUseCase = new(_cartRepositoryMock.Object);
        _deleteAllItemsUseCase.SetPresenter(_presenterMock.Object);
    }

    [Fact]
    public async Task ExecuteAsync_ExitingCart_MustRemoveAllQuantities()
    {
        Cart cart = new CartFixture().GenerateValidCart();
        cart.AddItem(new ItemFixture().GenerateValidItem());
        _cartRepositoryMock.Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Cart?>(cart));

        await _deleteAllItemsUseCase.ExecuteAsync(cart.UserId);

        Assert.True(cart.Items.Count == 0);
        _cartRepositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Once);
        _presenterMock.Verify(x => x.Success(null), Times.Once);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Never);
    }

    [Fact]
    public async Task ExecuteAsync_NotExitingCart_MustReturnBadRequest()
    {
        await _deleteAllItemsUseCase.ExecuteAsync(Guid.NewGuid());

        _cartRepositoryMock.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Never);
        _presenterMock.Verify(x => x.Success(null), Times.Never);
        _presenterMock.Verify(x => x.BadRequest(It.IsAny<object>()), Times.Once);
    }
}
