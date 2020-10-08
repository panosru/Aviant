namespace Aviant.DDD.Core.Events
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventBus;
    using Polly;

    public abstract class EventHandler<TEvent> : IEventHandler<TEvent>
    {
        #region IEventHandler<TEvent> Members

        public abstract Task Handle(EventReceived<TEvent> @event, CancellationToken cancellationToken);

        public virtual IAsyncPolicy RetryPolicy() => Policy.NoOpAsync();

        #endregion
    }
}