using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.Interfaces.UseCases;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.RemoveItemQuantity;

public sealed class RemoveItemQuantityUseCase : IUseCase<RemoveItemQuantityRequest>
{
    private readonly ICartRepository _repository;
    private IPresenter _presenter = default!;

    public RemoveItemQuantityUseCase(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(RemoveItemQuantityRequest request, CancellationToken cancellationToken)
    {
        Cart? cart = await _repository.GetCartByUserIdAsync(request.UserId, cancellationToken)
            ?? throw new ArgumentNullException();

        Item? existingItem = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId)
            ?? throw new ArgumentNullException();

        if (existingItem.Quantity == request.Quantity)
        {
            cart.RemoveItem(existingItem);
        }
        else
        {
            existingItem.RemoveQuantity(request.Quantity);
        }

        await _repository.UpdateCartAsync(cart, cancellationToken);
        _presenter.Success();
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
    }
}
