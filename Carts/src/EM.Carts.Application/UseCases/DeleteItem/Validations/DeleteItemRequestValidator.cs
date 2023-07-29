using EM.Carts.Domain;
using FluentValidation;

namespace EM.Carts.Application.UseCases.DeleteItem.Validations;

public sealed class DeleteItemRequestValidator : AbstractValidator<DeleteItemRequest>
{
    public DeleteItemRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(default(Guid))
            .WithMessage(ErrorMessage.UserIdInvalid);

        RuleFor(x => x.ProductId)
            .NotEqual(default(Guid))
            .WithMessage(ErrorMessage.ProductIdInvalid);
    }
}
