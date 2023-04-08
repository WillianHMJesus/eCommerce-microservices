using EM.Carts.Application.DTOs;
using EM.Carts.Application.Interfaces;

namespace EM.Carts.Application.UseCases.GetCartByUserId;

public interface IGetCartByUserIdUseCase
{
    Task ExecuteAsync(Guid userId);
    void SetPresenter(IPresenter presenter);
}
