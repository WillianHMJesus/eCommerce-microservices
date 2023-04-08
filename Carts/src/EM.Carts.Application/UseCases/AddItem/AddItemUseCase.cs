using EM.Carts.Application.Interfaces;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.AddItem;

public class AddItemUseCase : IAddItemUseCase
{
    private readonly ICartRepository _cartRepository;
    private IPresenter _presenter = default!;

    public AddItemUseCase(ICartRepository cartRepository)
        => _cartRepository = cartRepository;

    public async Task ExecuteAsync(AddItemRequest request, Guid userId)
    {
        Cart? cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
        {
            cart = new Cart(userId);
            await _cartRepository.AddCartAsync(cart);
        }

        Item item = new(request.ProductId, request.ProductName, request.ProductImage, request.Value, request.Quantity);
        cart.AddItem(item);

        await _cartRepository.UpdateCartAsync(cart);
        _presenter.Success();
    }

    public void SetPresenter(IPresenter presenter)
        => _presenter = presenter;
}
