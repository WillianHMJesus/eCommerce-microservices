using EM.Carts.Application.Interfaces;

namespace EM.Carts.Application.UseCases.SubtractItemQuantity;

public interface ISubtractItemQuantityUseCase
{
    Task ExecuteAsync(SubtractItemQuantityRequest request, Guid userId);
    void SetPresenter(IPresenter presenter);
}
