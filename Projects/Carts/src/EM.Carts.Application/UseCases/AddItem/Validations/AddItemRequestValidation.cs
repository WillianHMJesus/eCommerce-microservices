using EM.Carts.Application.Interfaces;
using FluentValidation.Results;

namespace EM.Carts.Application.UseCases.AddItem.Validations;

public sealed class AddItemRequestValidation : IAddItemUseCase
{
    private readonly IAddItemUseCase _addItemUseCase;
    private IPresenter _presenter = default!;

    public AddItemRequestValidation(IAddItemUseCase addItemUseCase)
        => _addItemUseCase = addItemUseCase;

    public async Task ExecuteAsync(AddItemRequest request)
    {
        AddItemRequestValidator validator = new();
        ValidationResult validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            _presenter.BadRequest(validationResult.Errors);
            return;
        }

        await _addItemUseCase.ExecuteAsync(request);
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
        _addItemUseCase.SetPresenter(presenter);
    }
}
