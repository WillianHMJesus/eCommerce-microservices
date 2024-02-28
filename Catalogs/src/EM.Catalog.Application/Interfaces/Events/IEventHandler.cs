using MediatR;

namespace EM.Catalog.Application.Interfaces.Events;

public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent
{ }
