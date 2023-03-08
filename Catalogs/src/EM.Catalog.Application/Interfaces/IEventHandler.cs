using MediatR;

namespace EM.Catalog.Application.Interfaces;

public interface IEventHandler<TEvent> : INotificationHandler<TEvent>
    where TEvent : IEvent
{ }
