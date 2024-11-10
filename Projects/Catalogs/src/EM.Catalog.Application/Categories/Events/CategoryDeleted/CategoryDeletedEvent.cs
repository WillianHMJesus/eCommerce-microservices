using EM.Catalog.Application.Interfaces.Events;

namespace EM.Catalog.Application.Categories.Events.CategoryDeleted;

public sealed record CategoryDeletedEvent(Guid Id)
    : IEvent
{ }
