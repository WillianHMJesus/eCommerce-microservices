using EM.Carts.Application.Validations;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Carts.Application.UseCases.AddItem;

public sealed class AddItemRequestValidator : AbstractValidator<AddItemRequest>
{
    private readonly IGenericValidations _validations;

    public AddItemRequestValidator(IGenericValidations validations)
    {
        _validations = validations;

        RuleFor(x => x.UserId)
            .NotEqual(default(Guid))
            .WithMessage(Key.UserIdInvalid);

        RuleFor(x => x.ProductId)
            .NotEqual(default(Guid))
            .WithMessage(Key.ProductInvalidId);

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage(Key.ProductQuantityLessThanEqualToZero);

        RuleFor(x => x.ProductId)
            .MustAsync(async (_, value, cancellationToken) => await _validations.ValidateProductAvailabilityAsync(value, cancellationToken))
            .WithMessage(Key.ProductUnavailable);
    }
}
