namespace Aviant.Core.DomainEvents;

using EventBus;
using Polly;

/// <inheritdoc />
public abstract class DomainEventHandler<TDomainEvent> : IDomainEventHandler<TDomainEvent>
{
    #region IDomainEventHandler<TDomainEvent> Members

    /// <summary>
    ///     Virtual retry policy, which does nothing if not overriden by the derived class.
    /// </summary>
    /// <returns>
    ///     <see cref="Policy" />
    /// </returns>
    public virtual IAsyncPolicy RetryPolicy() => Policy.NoOpAsync();

    /// <inheritdoc />
    /// <summary>
    ///     Handles the event data
    /// </summary>
    /// <param name="event">The current event object</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns></returns>
    public abstract Task Handle(EventReceived<TDomainEvent> @event, CancellationToken cancellationToken);

    #endregion
}
