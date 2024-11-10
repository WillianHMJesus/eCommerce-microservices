using EM.Carts.Application.UseCases.DeleteAllItems;
using EM.Common.Core.ResourceManagers;
using FluentValidation;

namespace EM.Carts.Application.UseCases.GetCartByUserId;

public sealed class GetCartByUserIdRequestValidator : AbstractValidator<GetCartByUserIdRequest>
{
    public GetCartByUserIdRequestValidator()
    {
        RuleFor(x => x.UserId)
            .NotEqual(default(Guid))
            .WithMessage(Key.UserIdInvalid);
    }
}
