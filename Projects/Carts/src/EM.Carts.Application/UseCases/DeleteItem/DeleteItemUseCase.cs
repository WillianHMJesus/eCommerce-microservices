using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.Interfaces.UseCases;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.DeleteItem;

public sealed class DeleteItemUseCase : IUseCase<DeleteItemRequest>
{
    private readonly ICartRepository _repository;
    private IPresenter _presenter = default!;

    public DeleteItemUseCase(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(DeleteItemRequest request, CancellationToken cancellationToken)
    {
        Cart cart = await _repository.GetCartByUserIdAsync(request.UserId, cancellationToken)
            ?? throw new ArgumentNullException();

        Item existingItem = cart.Items.FirstOrDefault(x => x.ProductId == request.ProductId)
            ?? throw new ArgumentNullException();

        cart.RemoveItem(existingItem);
        await _repository.UpdateCartAsync(cart, cancellationToken);

        _presenter.Success();
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
    }
}
