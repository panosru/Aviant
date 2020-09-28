namespace Aviant.DDD.Core.EventBus
{
    using MediatR;

    public sealed class EventReceived<TEvent> : INotification
    {
        public EventReceived(TEvent @event) => Event = @event;

        public TEvent Event { get; }
    }
}