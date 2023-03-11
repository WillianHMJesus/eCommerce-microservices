using MediatR;

namespace EM.Catalog.Application.Interfaces;

public interface IQuery<T> : IRequest<T>
{ }
