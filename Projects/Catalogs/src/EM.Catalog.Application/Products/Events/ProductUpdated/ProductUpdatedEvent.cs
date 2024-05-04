using EM.Catalog.Application.Interfaces.Events;
using EM.Catalog.Domain.Entities;

namespace EM.Catalog.Application.Products.Events.ProductUpdated;

public sealed record ProductUpdatedEvent(Guid Id, string Name, string Description, decimal Value, short Quantity, string Image, bool Available, Category Category)
    : IEvent
{ }
