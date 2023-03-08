using EM.Catalog.Application.Results;
using MediatR;

namespace EM.Catalog.Application.Interfaces;

public interface ICommand : IRequest<Result>
{ }
