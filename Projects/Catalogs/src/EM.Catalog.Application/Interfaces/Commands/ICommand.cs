using EM.Common.Core.ResourceManagers;
using MediatR;

namespace EM.Catalog.Application.Interfaces;

public interface ICommand : IRequest<Result>
{ }
