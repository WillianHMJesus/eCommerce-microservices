using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.Interfaces.UseCases;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.AddItemQuantity;

public sealed class AddItemQuantityUseCase : IUseCase<AddItemQuantityRequest>
{
    private readonly ICartRepository _repository;
    private IPresenter _presenter = default!;

    public AddItemQuantityUseCase(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(AddItemQuantityRequest request, CancellationToken cancellationToken)
    {
        Cart cart = await _repository.GetCartByUserIdAsync(request.UserId, cancellationToken)
            ?? throw new ArgumentNullException();

        Item? existingItem = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId)
            ?? throw new ArgumentNullException();

        existingItem.AddQuantity(request.Quantity);
        await _repository.UpdateCartAsync(cart, cancellationToken);

        _presenter.Success();
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
    }
}
