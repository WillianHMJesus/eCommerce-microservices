using EM.Carts.Application.Interfaces;
using EM.Carts.Application.UseCases.AddItem;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;
using EM.Carts.UnitTests.Fixtures.Application;
using EM.Carts.UnitTests.Fixtures.Domain;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace EM.Carts.UnitTests.Application;

public sealed class AddItemUseCaseTest
{
    private readonly Mock<ICartRepository> _mockCartRepository;
    private readonly Mock<IPresenter> _mockPresenter;
    private readonly AddItemUseCase _addItemUseCase;
    private readonly AddItemRequest _addItemRequest;
    private readonly Cart _cart;

    public AddItemUseCaseTest()
    {
        _mockCartRepository = new();
        _mockPresenter = new();
        _addItemUseCase = new(_mockCartRepository.Object);
        _addItemUseCase.SetPresenter(_mockPresenter.Object);

        AddItemRequestFixture addItemRequestFixture = new();
        _addItemRequest = addItemRequestFixture.GenerateValidAddItemRequest();
        _cart = new(Guid.NewGuid());
    }

    [Fact]
    public async Task ExecuteAsync_NewCart_MustAddNewCart()
    {
        await _addItemUseCase.ExecuteAsync(_addItemRequest);

        _mockCartRepository.Verify(x => x.AddCartAsync(It.IsAny<Cart>()), Times.Once);
        _mockCartRepository.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ExistingCartNewItem_MustAddNewItem()
    {
        _mockCartRepository.Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Cart?>(_cart));

        await _addItemUseCase.ExecuteAsync(_addItemRequest);

        Assert.True(_cart.Items.Count == 1);
        _mockCartRepository.Verify(x => x.AddCartAsync(It.IsAny<Cart>()), Times.Never);
        _mockCartRepository.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Once);
    }

    [Fact]
    public async Task ExecuteAsync_ExistingCartAndItem_MustAddQuantityItem()
    {
        Item item = new ItemFixture().GenerateValidItem();
        int itemQuantity = item.Quantity;
        _cart.AddItem(item);
        _mockCartRepository.Setup(x => x.GetCartByUserIdAsync(It.IsAny<Guid>())).Returns(Task.FromResult<Cart?>(_cart));
        _addItemRequest.ProductId = item.ProductId;

        await _addItemUseCase.ExecuteAsync(_addItemRequest);

        Assert.Equal(itemQuantity + _addItemRequest.Quantity, _cart.Items[0].Quantity);
        _mockCartRepository.Verify(x => x.AddCartAsync(It.IsAny<Cart>()), Times.Never);
        _mockCartRepository.Verify(x => x.UpdateCartAsync(It.IsAny<Cart>()), Times.Once);
    }
}
