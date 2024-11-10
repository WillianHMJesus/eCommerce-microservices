using EM.Catalog.Application.Interfaces.Events;

namespace EM.Catalog.Application.Categories.Events.CategoryUpdated;

public sealed record CategoryUpdatedEvent(Guid Id, short Code, string Name, string Description) 
    : IEvent
{ }
