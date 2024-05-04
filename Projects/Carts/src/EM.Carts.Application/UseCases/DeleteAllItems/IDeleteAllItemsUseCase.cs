using EM.Carts.Application.Interfaces;

namespace EM.Carts.Application.UseCases.DeleteAllItems;

public interface IDeleteAllItemsUseCase
{
    Task ExecuteAsync(Guid userId);
    void SetPresenter(IPresenter presenter);
}
