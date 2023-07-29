using EM.Carts.Application.DTOs;
using EM.Carts.Application.Interfaces;
using EM.Carts.Domain.Entities;
using EM.Carts.Domain.Interfaces;

namespace EM.Carts.Application.UseCases.GetCartByUserId;

public sealed class GetCartByUserIdUseCase : IGetCartByUserIdUseCase
{
    private readonly ICartRepository _cartRepository;
    private IPresenter _presenter = default!;

    public GetCartByUserIdUseCase(ICartRepository cartRepository)
        => _cartRepository = cartRepository;

    public async Task ExecuteAsync(Guid userId)
    {
        Cart? cart = await _cartRepository.GetCartByUserIdAsync(userId);
        CartDTO? cartDTO = cart != null ? (CartDTO)cart : null;

        _presenter.Success(cartDTO);
    }

    public void SetPresenter(IPresenter presenter)
        => _presenter = presenter;
}
