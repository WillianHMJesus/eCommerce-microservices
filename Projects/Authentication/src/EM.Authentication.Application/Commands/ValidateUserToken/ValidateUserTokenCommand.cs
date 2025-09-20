using WH.SharedKernel.Mediator;

namespace EM.Authentication.Application.Commands.ValidateUserToken;

public sealed record ValidateUserTokenCommand(Guid UserTokenId, string Token)
    : ICommand
{ }
