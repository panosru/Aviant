namespace Aviant.DDD.Application.EventBus
{
    #region

    using Domain.EventBus;

    #endregion

    public static class EventReceivedFactory
    {
        public static EventReceived<TEvent> Create<TEvent>(TEvent @event) => new EventReceived<TEvent>(@event);
    }
}