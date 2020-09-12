namespace Aviant.DDD.Domain.EventBus
{
    #region

    using MediatR;

    #endregion

    public class EventReceived<TEvent> : INotification
    {
        public EventReceived(TEvent @event) => Event = @event;

        public TEvent Event { get; }
    }
}