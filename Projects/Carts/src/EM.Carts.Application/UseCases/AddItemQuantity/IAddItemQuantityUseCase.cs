using EM.Carts.Application.Interfaces;

namespace EM.Carts.Application.UseCases.AddItemQuantity;

public interface IAddItemQuantityUseCase
{
    Task ExecuteAsync(AddItemQuantityRequest request);
    void SetPresenter(IPresenter presenter);
}
