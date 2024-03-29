using Aviant.Core.EventSourcing.Aggregates;
using Aviant.Core.Timing;

namespace Aviant.Core.EventSourcing.DomainEvents;

/// <inheritdoc cref="IDomainEvent{TAggregateId}" />
public abstract record DomainEvent<TAggregate, TAggregateId> : IDomainEvent<TAggregateId>
    where TAggregate : IAggregate<TAggregateId>
    where TAggregateId : IAggregateId
{
    #pragma warning disable 8618
    /// <summary>
    ///     Constructor required by reflection
    /// </summary>
    protected DomainEvent()
    { }
    #pragma warning restore 8618

    /// <summary>
    ///     Domain event constructor
    /// </summary>
    /// <param name="aggregate">The aggregate object</param>
    /// <exception cref="ArgumentNullException">Can be thrown if aggregate is null</exception>
    protected DomainEvent(TAggregate aggregate)
    {
        if (aggregate is null)
            throw new ArgumentNullException(nameof(aggregate));

        AggregateVersion = aggregate.Version;
        AggregateId      = aggregate.Id;
        Occurred         = Clock.Now;
    }

    #region IDomainEvent<TAggregateId> Members

    /// <inheritdoc />
    public long AggregateVersion { get; private set; }

    /// <inheritdoc />
    public TAggregateId AggregateId { get; private set; }

    /// <inheritdoc />
    public DateTime Occurred { get; private set; }

    #endregion
}
