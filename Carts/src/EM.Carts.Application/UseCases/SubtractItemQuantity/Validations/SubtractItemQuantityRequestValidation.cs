using EM.Carts.Application.Interfaces;
using FluentValidation.Results;

namespace EM.Carts.Application.UseCases.SubtractItemQuantity.Validations;

public class SubtractItemQuantityRequestValidation : ISubtractItemQuantityUseCase
{
    private readonly ISubtractItemQuantityUseCase _subtractItemQuantityUseCase;
    private IPresenter _presenter = default!;

    public SubtractItemQuantityRequestValidation(ISubtractItemQuantityUseCase subtractItemQuantityUseCase)
        => _subtractItemQuantityUseCase = subtractItemQuantityUseCase;

    public async Task ExecuteAsync(SubtractItemQuantityRequest request, Guid userId)
    {
        SubtractItemQuantityRequestValidator validator = new();
        ValidationResult validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            _presenter.BadRequest(validationResult.Errors);
            return;
        }

        await _subtractItemQuantityUseCase.ExecuteAsync(request, userId);
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
        _subtractItemQuantityUseCase.SetPresenter(presenter);
    }
}
