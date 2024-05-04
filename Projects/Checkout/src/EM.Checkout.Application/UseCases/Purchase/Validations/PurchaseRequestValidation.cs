using EM.Checkout.Application.Interfaces;
using FluentValidation.Results;

namespace EM.Checkout.Application.UseCases.Purchase.Validations;

public sealed class PurchaseRequestValidation : IPurchaseUseCase
{
    private readonly IPurchaseUseCase _purchaseUseCase;
    private IPresenter _presenter = null!;

    public PurchaseRequestValidation(IPurchaseUseCase purchaseUseCase)
        => _purchaseUseCase = purchaseUseCase;

    public async Task ExecuteAsync(PurchaseRequest request, CancellationToken cancellationToken)
    {
        PurchaseRequestValidator validator = new();
        ValidationResult validationResult = validator.Validate(request);

        if(!validationResult.IsValid)
        {
            _presenter.BadRequest(validationResult.Errors);
            return;
        }

        await _purchaseUseCase.ExecuteAsync(request, cancellationToken);
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
        _purchaseUseCase.SetPresenter(presenter);
    }
}
