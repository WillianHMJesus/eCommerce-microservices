using WH.SharedKernel.Mediator;

namespace EM.Authentication.Application.Commands.ChangeUserPassword;

public sealed record ChangeUserPasswordCommand(string EmailAddress, string OldPassword, string NewPassword, string ConfirmPassword)
    : ICommand
{ }
