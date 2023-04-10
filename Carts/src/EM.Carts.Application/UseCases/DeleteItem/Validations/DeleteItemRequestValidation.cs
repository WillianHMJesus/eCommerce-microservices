﻿using EM.Carts.Application.Interfaces;
using FluentValidation.Results;

namespace EM.Carts.Application.UseCases.DeleteItem.Validations;

public class DeleteItemRequestValidation : IDeleteItemUseCase
{
    private readonly IDeleteItemUseCase _deleteItemUseCase;
    private IPresenter _presenter = default!;

    public DeleteItemRequestValidation(IDeleteItemUseCase deleteItemUseCase)
        => _deleteItemUseCase = deleteItemUseCase;

    public async Task ExecuteAsync(DeleteItemRequest request, Guid userId)
    {
        DeleteItemRequestValidator validator = new();
        ValidationResult validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            _presenter.BadRequest(validationResult.Errors);
            return;
        }

        await _deleteItemUseCase.ExecuteAsync(request, userId);
    }

    public void SetPresenter(IPresenter presenter)
    {
        _presenter = presenter;
        _deleteItemUseCase.SetPresenter(presenter);
    }
}
