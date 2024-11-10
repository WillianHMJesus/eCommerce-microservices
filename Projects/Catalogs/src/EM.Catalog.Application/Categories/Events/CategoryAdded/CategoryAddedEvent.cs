using EM.Catalog.Application.Interfaces.Events;

namespace EM.Catalog.Application.Categories.Events.CategoryAdded;

public sealed record CategoryAddedEvent(Guid Id, short Code, string Name, string Description) 
    : IEvent
{ }
