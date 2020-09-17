namespace Aviant.DDD.Core.EventBus
{
    using MediatR;

    public class EventReceived<TEvent> : INotification
    {
        public EventReceived(TEvent @event) => Event = @event;

        public TEvent Event { get; }
    }
}