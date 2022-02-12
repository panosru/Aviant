namespace Aviant.EventSourcing.Core.DomainEvents;

using Aviant.Foundation.Core.Services;
using EventBus;
using MediatR;

internal interface IDomainEventHandler<TDomainEvent>
    : INotificationHandler<EventReceived<TDomainEvent>>,
      IRetry
{ }
