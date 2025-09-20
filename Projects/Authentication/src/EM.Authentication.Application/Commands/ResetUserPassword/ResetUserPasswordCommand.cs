using WH.SharedKernel.Mediator;

namespace EM.Authentication.Application.Commands.ResetUserPassword;

public sealed record ResetUserPasswordCommand(Guid UserTokenId, string NewPassword, string ConfirmPassword)
    : ICommand
{ }
