using EM.Carts.Application.Interfaces.Presenters;

namespace EM.Carts.Application.Interfaces.UseCases;

public interface IUseCase<TRequest> where TRequest : IRequest
{
    Task ExecuteAsync(TRequest request, CancellationToken cancellationToken);
    void SetPresenter(IPresenter presenter);
}
