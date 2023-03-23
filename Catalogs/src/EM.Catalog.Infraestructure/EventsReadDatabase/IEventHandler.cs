using MediatR;

namespace EM.Catalog.Infraestructure.EventsReadDatabase;

public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent
{ }
