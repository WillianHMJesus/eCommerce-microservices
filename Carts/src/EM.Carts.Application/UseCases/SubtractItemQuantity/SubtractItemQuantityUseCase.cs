using EM.Carts.Application.Interfaces;
using EM.Carts.Domain;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.SubtractItemQuantity;

public class SubtractItemQuantityUseCase : ISubtractItemQuantityUseCase
{
    private readonly ICartRepository _cartRepository;
    private IPresenter _presenter = default!;

    public SubtractItemQuantityUseCase(ICartRepository cartRepository)
        => _cartRepository = cartRepository;

    public async Task ExecuteAsync(SubtractItemQuantityRequest request, Guid userId)
    {
        Cart? cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
        {
            _presenter.BadRequest(new
            {
                ErrorMessage = ErrorMessage.CartNotFound
            });

            return;
        }

        Item? existingItem = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId);

        if (existingItem == null)
        {
            _presenter.BadRequest(new
            {
                ErrorMessage = ErrorMessage.ItemNotFound
            });

            return;
        }

        if (existingItem.Quantity == request.Quantity)
        {
            cart.RemoveItem(existingItem);
        }
        else
        {
            existingItem.SubtractQuantity(request.Quantity);
        }

        await _cartRepository.UpdateCartAsync(cart);
        _presenter.Success();
    }

    public void SetPresenter(IPresenter presenter)
        => _presenter = presenter;
}
