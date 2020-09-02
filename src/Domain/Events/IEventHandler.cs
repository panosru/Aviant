namespace Aviant.DDD.Domain.Events
{
    using EventBus;
    using MediatR;

    public interface IEventHandler<TEvent> : INotificationHandler<EventReceived<TEvent>>
    {}
}