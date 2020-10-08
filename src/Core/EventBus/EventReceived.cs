namespace Aviant.DDD.Core.EventBus
{
    using MediatR;

    public sealed class EventReceived<TDomainEvent> : INotification
    {
        public EventReceived(TDomainEvent @event) => Event = @event;

        public TDomainEvent Event { get; }
    }
}