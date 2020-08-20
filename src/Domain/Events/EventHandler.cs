namespace Aviant.DDD.Domain.Events
{
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
        where TEvent : IEvent
    {
        public abstract Task Handle(TEvent @event, CancellationToken cancellationToken);
    }
}