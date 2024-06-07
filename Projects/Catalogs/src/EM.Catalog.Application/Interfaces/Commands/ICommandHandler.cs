using EM.Common.Core.ResourceManagers;
using MediatR;

namespace EM.Catalog.Application.Interfaces;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{ }
