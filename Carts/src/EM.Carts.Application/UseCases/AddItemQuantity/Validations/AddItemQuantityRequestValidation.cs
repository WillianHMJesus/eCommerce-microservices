﻿using EM.Carts.Application.Interfaces;
using FluentValidation.Results;

namespace EM.Carts.Application.UseCases.AddItemQuantity.Validations;

public sealed class AddItemQuantityRequestValidation : IAddItemQuantityUseCase
{
    private readonly IAddItemQuantityUseCase _addItemQuantityUseCase;
    private IPresenter _presenter = default!;

    public AddItemQuantityRequestValidation(IAddItemQuantityUseCase addItemQuantityUseCase)
        => _addItemQuantityUseCase = addItemQuantityUseCase;

    public async Task ExecuteAsync(AddItemQuantityRequest request)
    {
        AddItemQuantityRequestValidator validator = new();
        ValidationResult validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            _presenter.BadRequest(validationResult.Errors);
            return;
        }

        await _addItemQuantityUseCase.ExecuteAsync(request);
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
        _addItemQuantityUseCase.SetPresenter(presenter);
    }
}
