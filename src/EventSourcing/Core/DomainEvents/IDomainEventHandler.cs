using Aviant.Core.EventSourcing.EventBus;
using Aviant.Core.Services;
using MediatR;

namespace Aviant.Core.EventSourcing.DomainEvents;

internal interface IDomainEventHandler<TDomainEvent>
    : INotificationHandler<EventReceived<TDomainEvent>>,
      IRetry
{ }
