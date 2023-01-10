using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Domain;

public interface IRepository<T> where T : IAggregateRoot
{ }
