namespace Aviant.DDD.Application.EventBus
{
    #region

    using Core.EventBus;

    #endregion

    public static class EventReceivedFactory
    {
        public static EventReceived<TEvent> Create<TEvent>(TEvent @event) => new EventReceived<TEvent>(@event);
    }
}