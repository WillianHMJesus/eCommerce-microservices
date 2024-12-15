using EM.Common.Core.ResourceManagers;
using MediatR;

namespace EM.Checkout.Application.Interfaces.Commands;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{ }
