using EM.Carts.Application.Interfaces;
using FluentValidation.Results;

namespace EM.Carts.Application.UseCases.AddItem.Validations;

public class AddItemRequestValidation : IAddItemUseCase
{
    private readonly IAddItemUseCase _addItemUseCase;
    private IPresenter _presenter = default!;

    public AddItemRequestValidation(IAddItemUseCase addItemUseCase)
        => _addItemUseCase = addItemUseCase;

    public async Task ExecuteAsync(AddItemRequest request, Guid userId)
    {
        AddItemRequestValidator validator = new();
        ValidationResult validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            _presenter.BadRequest(validationResult.Errors);
            return;
        }

        await _addItemUseCase.ExecuteAsync(request, userId);
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
        _addItemUseCase.SetPresenter(presenter);
    }
}
