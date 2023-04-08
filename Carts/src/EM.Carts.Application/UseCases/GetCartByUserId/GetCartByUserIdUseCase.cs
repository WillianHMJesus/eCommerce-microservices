using EM.Carts.Application.DTOs;
using EM.Carts.Application.Interfaces;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.GetCartByUserId;

public class GetCartByUserIdUseCase : IGetCartByUserIdUseCase
{
    private readonly ICartRepository _cartRepository;
    private IPresenter _presenter = default!;

    public GetCartByUserIdUseCase(ICartRepository cartRepository)
        => _cartRepository = cartRepository;

    public async Task ExecuteAsync(Guid userId)
    {
        Cart? cart = await _cartRepository.GetCartByUserIdAsync(userId);

        if (cart == null)
        {
            _presenter.Success();
            return;
        }

        _presenter.Success((CartDTO)cart);
    }

    public void SetPresenter(IPresenter presenter)
        => _presenter = presenter;
}
