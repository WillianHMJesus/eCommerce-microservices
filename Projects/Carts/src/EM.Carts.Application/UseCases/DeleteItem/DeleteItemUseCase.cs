using EM.Carts.Application.Interfaces;
using EM.Carts.Domain;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.DeleteItem;

public sealed class DeleteItemUseCase : IDeleteItemUseCase
{
    private readonly ICartRepository _cartRepository;
    private IPresenter _presenter = default!;

    public DeleteItemUseCase(ICartRepository cartRepository)
        => _cartRepository = cartRepository;

    public async Task ExecuteAsync(DeleteItemRequest request)
    {
        Cart? cart = await _cartRepository.GetCartByUserIdAsync(request.UserId);

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

        cart.RemoveItem(existingItem);
        await _cartRepository.UpdateCartAsync(cart);

        _presenter.Success();
    }

    public void SetPresenter(IPresenter presenter)
        => _presenter = presenter;
}
