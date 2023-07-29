using EM.Carts.Domain;
using FluentValidation;

namespace EM.Carts.Application.UseCases.AddItem.Validations;

public sealed class AddItemRequestValidator : AbstractValidator<AddItemRequest>
{
    public AddItemRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(default(Guid))
            .WithMessage(ErrorMessage.UserIdInvalid);

        RuleFor(x => x.ProductId)
            .NotEqual(default(Guid))
            .WithMessage(ErrorMessage.ProductIdInvalid);

        RuleFor(x => x.ProductName)
            .NotEmpty()
            .NotNull()
            .WithMessage(ErrorMessage.ProductNameNullOrEmpty);

        RuleFor(x => x.ProductImage)
            .NotEmpty()
            .NotNull()
            .WithMessage(ErrorMessage.ProductImageNullOrEmpty);

        RuleFor(x => x.Value)
            .NotEqual(default(decimal))
            .WithMessage(ErrorMessage.ValueLessThanEqualToZero);

        RuleFor(x => x.Quantity)
            .NotEqual(default(int))
            .WithMessage(ErrorMessage.QuantityLessThanEqualToZero);
    }
}
