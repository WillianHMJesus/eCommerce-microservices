using MediatR;

namespace EM.Catalog.Infraestructure.Interfaces;

public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent
{ }
