using Aviant.Core.Entities;
using Aviant.Core.EventSourcing.DomainEvents;

namespace Aviant.Core.EventSourcing.Aggregates;

/// <inheritdoc />
/// <summary>
///     The interface that defines the aggregate object
/// </summary>
/// <typeparam name="TAggregateId">The current aggregate id object</typeparam>
public interface IAggregate<out TAggregateId> : IEntity<TAggregateId>
    where TAggregateId : IAggregateId
{
    /// <summary>
    ///     The version of the aggregate
    /// </summary>
    public long Version { get; }

    /// <summary>
    ///     The collection of aggregate events
    /// </summary>
    public IReadOnlyCollection<IDomainEvent<TAggregateId>> Events { get; }

    /// <summary>
    ///     Clears the collection of events
    /// </summary>
    public void ClearEvents();
}
