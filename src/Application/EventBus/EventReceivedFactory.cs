namespace Aviant.DDD.Application.EventBus
{
    public static class EventReceivedFactory
    {
        public static EventReceived<TEvent> Create<TEvent>(TEvent @event)
        {
            return new EventReceived<TEvent>(@event);
        }
    }
}