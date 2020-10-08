namespace Aviant.DDD.Core.DomainEvents
{
    using EventBus;
    using MediatR;
    using Services;

    internal interface IDomainEventHandler<TEvent>
        : INotificationHandler<EventReceived<TEvent>>,
          IRetry
    { }
}