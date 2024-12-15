using EM.Common.Core.ResourceManagers;
using MediatR;

namespace EM.Checkout.Application.Interfaces.Commands;

public interface ICommand : IRequest<Result>
{ }
