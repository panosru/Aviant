namespace Aviant.DDD.Application.EventBus
{
    using Core.EventBus;

    internal static class EventReceivedFactory
    {
        public static EventReceived<TDomainEvent> Create<TDomainEvent>(TDomainEvent @event) =>
            new(@event);
    }
}