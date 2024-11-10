using EM.Carts.Application.Validations;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Carts.Application.UseCases.DeleteItem;

public sealed class DeleteItemRequestValidator : AbstractValidator<DeleteItemRequest>
{
    private readonly IGenericValidations _validations;

    public DeleteItemRequestValidator(IGenericValidations validations)
    {
        _validations = validations;

        RuleFor(x => x.UserId)
            .NotEqual(default(Guid))
            .WithMessage(Key.UserIdInvalid);

        RuleFor(x => x.ProductId)
            .NotEqual(default(Guid))
            .WithMessage(Key.ProductInvalidId);

        RuleFor(x => x.UserId)
            .MustAsync(async (_, value, cancellationToken) => await _validations.ValidateCartByUserIdAsync(value, cancellationToken))
            .WithMessage(Key.CartNotFound);

        RuleFor(x => x)
            .MustAsync(async (_, value, cancellationToken) => await _validations.ValidateItemByProductIdAsync(value.ProductId, value.UserId, cancellationToken))
            .WithMessage(Key.ItemNotFound);
    }
}
