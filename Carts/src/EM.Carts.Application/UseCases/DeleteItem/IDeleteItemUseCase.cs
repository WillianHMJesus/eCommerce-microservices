using EM.Carts.Application.Interfaces;

namespace EM.Carts.Application.UseCases.DeleteItem;

public interface IDeleteItemUseCase
{
    Task ExecuteAsync(DeleteItemRequest request, Guid userId);
    void SetPresenter(IPresenter presenter);
}
