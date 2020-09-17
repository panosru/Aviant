namespace Aviant.DDD.Core.Events
{
    using EventBus;
    using MediatR;

    public interface IEventHandler<TEvent> : INotificationHandler<EventReceived<TEvent>>
    { }
}