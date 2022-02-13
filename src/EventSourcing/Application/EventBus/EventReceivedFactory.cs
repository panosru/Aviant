namespace Aviant.Application.EventSourcing.EventBus;

using Core.EventSourcing.EventBus;

internal static class EventReceivedFactory
{
    public static EventReceived<TDomainEvent> Create<TDomainEvent>(TDomainEvent @event) =>
        new(@event);
}
