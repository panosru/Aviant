namespace Aviant.DDD.Core.Events
{
    using EventBus;
    using MediatR;
    using Services;

    internal interface IEventHandler<TEvent>
        : INotificationHandler<EventReceived<TEvent>>,
          IRetry
    { }
}