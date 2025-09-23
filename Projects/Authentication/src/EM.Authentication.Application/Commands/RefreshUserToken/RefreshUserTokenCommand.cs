using WH.SharedKernel.Mediator;

namespace EM.Authentication.Application.Commands.RefreshUserToken;

public sealed record RefreshUserTokenCommand(Guid UserId)
    : ICommand
{ }
