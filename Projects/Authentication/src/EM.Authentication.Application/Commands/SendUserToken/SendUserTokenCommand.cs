using WH.SharedKernel.Mediator;

namespace EM.Authentication.Application.Commands.SendUserToken;

public sealed record SendUserTokenCommand(string EmailAddress)
    : ICommand
{ }
