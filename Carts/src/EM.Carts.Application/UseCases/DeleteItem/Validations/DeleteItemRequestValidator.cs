using EM.Carts.Domain;
using FluentValidation;

namespace EM.Carts.Application.UseCases.DeleteItem.Validations;

public class DeleteItemRequestValidator : AbstractValidator<DeleteItemRequest>
{
    public DeleteItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEqual(default(Guid))
            .WithMessage(ErrorMessage.ProductIdInvalid);
    }
}
