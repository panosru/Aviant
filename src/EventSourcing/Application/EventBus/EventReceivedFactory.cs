using Aviant.Core.EventSourcing.EventBus;

namespace Aviant.Application.EventSourcing.EventBus;

internal static class EventReceivedFactory
{
    public static EventReceived<TDomainEvent> Create<TDomainEvent>(TDomainEvent @event) =>
        new(@event);
}
