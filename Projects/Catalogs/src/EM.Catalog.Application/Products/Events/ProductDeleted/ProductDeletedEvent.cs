using EM.Catalog.Application.Interfaces.Events;

namespace EM.Catalog.Application.Products.Events.ProductDeleted;

public sealed record ProductDeletedEvent(Guid Id)
    : IEvent
{ }
