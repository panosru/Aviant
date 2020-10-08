namespace Aviant.DDD.Core.DomainEvents
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventBus;
    using Polly;

    public abstract class DomainEventHandler<TEvent> : IDomainEventHandler<TEvent>
    {
        #region IDomainEventHandler<TEvent> Members

        public abstract Task Handle(EventReceived<TEvent> @event, CancellationToken cancellationToken);

        public virtual IAsyncPolicy RetryPolicy() => Policy.NoOpAsync();

        #endregion
    }
}