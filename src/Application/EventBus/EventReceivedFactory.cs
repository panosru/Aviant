namespace Aviant.DDD.Application.EventBus
{
    using Core.EventBus;

    internal static class EventReceivedFactory
    {
        public static EventReceived<TEvent> Create<TEvent>(TEvent @event) => new EventReceived<TEvent>(@event);
    }
}