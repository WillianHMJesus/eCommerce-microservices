using EM.Carts.Application.Interfaces;

namespace EM.Carts.Application.UseCases.AddItemQuantity;

public interface IAddItemQuantityUseCase
{
    Task ExecuteAsync(AddItemQuantityRequest request, Guid userId);
    void SetPresenter(IPresenter presenter);
}
