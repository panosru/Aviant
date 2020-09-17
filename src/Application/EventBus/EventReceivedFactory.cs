namespace Aviant.DDD.Application.EventBus
{
    using Core.EventBus;

    public static class EventReceivedFactory
    {
        public static EventReceived<TEvent> Create<TEvent>(TEvent @event) => new EventReceived<TEvent>(@event);
    }
}