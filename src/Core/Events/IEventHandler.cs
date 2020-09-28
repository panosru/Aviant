namespace Aviant.DDD.Core.Events
{
    using EventBus;
    using MediatR;

    internal interface IEventHandler<TEvent> : INotificationHandler<EventReceived<TEvent>>
    { }
}