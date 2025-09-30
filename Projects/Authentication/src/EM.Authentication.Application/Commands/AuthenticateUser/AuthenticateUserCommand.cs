using WH.SharedKernel.Abstractions;

namespace EM.Authentication.Application.Commands.AuthenticateUser;

public sealed record AuthenticateUserCommand(string EmailAddress, string Password)
    : ICommand
{ }
