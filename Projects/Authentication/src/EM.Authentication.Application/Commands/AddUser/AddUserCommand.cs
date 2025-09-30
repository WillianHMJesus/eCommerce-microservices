using WH.SharedKernel.Abstractions;

namespace EM.Authentication.Application.Commands.AddUser;

public sealed record AddUserCommand(string UserName, string EmailAddress, string Password, string ConfirmPassword, string ProfileName)
    : ICommand
{ }
