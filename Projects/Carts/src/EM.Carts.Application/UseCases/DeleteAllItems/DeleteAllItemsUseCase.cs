using EM.Carts.Application.Interfaces.Presenters;
using EM.Carts.Application.Interfaces.UseCases;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.DeleteAllItems;

public sealed class DeleteAllItemsUseCase : IUseCase<DeleteAllItemsRequest>
{
    private readonly ICartRepository _repository;
    private IPresenter _presenter = default!;

    public DeleteAllItemsUseCase(ICartRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(DeleteAllItemsRequest request, CancellationToken cancellationToken)
    {
        Cart cart = await _repository.GetCartByUserIdAsync(request.UserId, cancellationToken)
            ?? throw new ArgumentNullException();

        cart.RemoveAllItems();
        await _repository.UpdateCartAsync(cart, cancellationToken);
        
        _presenter.Success();
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
    }
}
