using EM.Catalog.Application.Results;
using MediatR;

namespace EM.Catalog.Application.Interfaces;

public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{ }
