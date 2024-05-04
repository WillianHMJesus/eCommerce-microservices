using EM.Checkout.Application.Interfaces;

namespace EM.Checkout.Application.UseCases.Purchase;

public interface IPurchaseUseCase
{
    Task ExecuteAsync(PurchaseRequest request, CancellationToken cancellationToken);
    void SetPresenter(IPresenter presenter);
}
