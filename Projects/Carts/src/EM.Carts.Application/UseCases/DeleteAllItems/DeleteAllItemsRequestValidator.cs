using EM.Carts.Application.Validations;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Carts.Application.UseCases.DeleteAllItems;

public sealed class DeleteAllItemsRequestValidator : AbstractValidator<DeleteAllItemsRequest>
{
    private readonly IGenericValidations _validations;

    public DeleteAllItemsRequestValidator(IGenericValidations validations)
    {
        _validations = validations;

        RuleFor(x => x.UserId)
            .NotEqual(default(Guid))
            .WithMessage(Key.UserIdInvalid);

        RuleFor(x => x.UserId)
            .MustAsync(async (_, value, cancellationToken) => await _validations.ValidateCartByUserIdAsync(value, cancellationToken))
            .WithMessage(Key.CartNotFound);
    }
}
