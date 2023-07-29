using EM.Carts.Application.Interfaces;

namespace EM.Carts.Application.UseCases.SubtractItemQuantity;

public interface ISubtractItemQuantityUseCase
{
    Task ExecuteAsync(SubtractItemQuantityRequest request);
    void SetPresenter(IPresenter presenter);
}
