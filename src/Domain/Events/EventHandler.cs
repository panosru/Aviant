namespace Aviant.DDD.Domain.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventBus;

    public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
    {
    #region IEventHandler<TEvent> Members

        public abstract Task Handle(EventReceived<TEvent> @event, CancellationToken cancellationToken);

    #endregion
    }
}