namespace Aviant.DDD.Core.DomainEvents
{
    using EventBus;
    using MediatR;
    using Services;

    internal interface IDomainEventHandler<TDomainEvent>
        : INotificationHandler<EventReceived<TDomainEvent>>,
          IRetry
    { }
}