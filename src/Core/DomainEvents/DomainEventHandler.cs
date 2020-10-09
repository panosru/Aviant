namespace Aviant.DDD.Core.DomainEvents
{
    using System.Threading;
    using System.Threading.Tasks;
    using EventBus;
    using Polly;

    public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
    {
        #region IDomainEventHandler<TDomainEvent> Members

        public virtual IAsyncPolicy RetryPolicy() => Policy.NoOpAsync();

        public abstract Task Handle(EventReceived<TDomainEvent> @event, CancellationToken cancellationToken);

        #endregion
    }
}