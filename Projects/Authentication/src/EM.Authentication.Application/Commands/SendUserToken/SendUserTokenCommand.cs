using WH.SharedKernel.Abstractions;

namespace EM.Authentication.Application.Commands.SendUserToken;

public sealed record SendUserTokenCommand(string EmailAddress)
    : ICommand
{ }
