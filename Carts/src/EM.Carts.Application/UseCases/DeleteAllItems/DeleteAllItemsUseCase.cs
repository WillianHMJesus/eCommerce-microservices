using EM.Carts.Application.Interfaces;
using EM.Carts.Domain;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.DeleteAllItems;

public sealed class DeleteAllItemsUseCase : IDeleteAllItemsUseCase
{
    private readonly ICartRepository _cartRepository;
    private IPresenter _presenter = default!;

    public DeleteAllItemsUseCase(ICartRepository cartRepository)
        => _cartRepository = cartRepository;

    public async Task ExecuteAsync(Guid userId)
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

        cart.RemoveAllItems();
        await _cartRepository.UpdateCartAsync(cart);
        
        _presenter.Success();
    }

    public void SetPresenter(IPresenter presenter)
        => _presenter = presenter;
}
