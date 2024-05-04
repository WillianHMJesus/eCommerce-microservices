using EM.Carts.Application.Interfaces;

namespace EM.Carts.Application.UseCases.AddItem;

public interface IAddItemUseCase
{
    Task ExecuteAsync(AddItemRequest request);
    void SetPresenter(IPresenter presenter);
}
