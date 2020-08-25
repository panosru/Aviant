namespace Aviant.DDD.Application.EventBus
{
    using MediatR;

    public class EventReceived<TEvent> : INotification
    {
        public TEvent Event { get; }
        
        public EventReceived(TEvent @event)
        {
            Event = @event;
        }
    }
}