// ReSharper disable InvalidXmlDocComment

namespace Aviant.Core.EventSourcing.Aggregates;

using System.Collections.Immutable;
using System.Reflection;
using Entities;
using DomainEvents;

/// <inheritdoc cref="Aviant.Core.Entities.Entity{TKey}" />
/// <inheritdoc cref="IAggregate{TAggregateId}" />
/// <summary>
///     The aggregate object
/// </summary>
/// <typeparam name="TAggregate">Aggregate object</typeparam>
/// <typeparam name="TAggregateId">Aggregate id</typeparam>
public abstract class Aggregate<TAggregate, TAggregateId>
    : Entity<TAggregateId>, IAggregate<TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    private readonly Queue<IDomainEvent<TAggregateId>> _events = new();

    /// <inheritdoc />
    /// <summary>
    ///     Aggregate constructor without parameters
    /// </summary>
    protected Aggregate()
    { }

    /// <inheritdoc />
    /// <summary>
    ///     Aggregate constructor
    /// </summary>
    /// <param name="aggregateId">Aggregate id object</param>
    protected Aggregate(TAggregateId aggregateId)
        : base(aggregateId)
    { }

    #region IAggregate<TAggregateId> Members

    /// <inheritdoc />
    public IReadOnlyCollection<IDomainEvent<TAggregateId>> Events => _events.ToImmutableArray();

    /// <inheritdoc />
    public long Version { get; private set; }

    /// <inheritdoc />
    public void ClearEvents()
    {
        _events.Clear();
    }

    #endregion

    /// <summary>
    ///     Adds an event into the aggregate
    /// </summary>
    /// <param name="event">DomainEvent object</param>
    protected void AddEvent(IDomainEvent<TAggregateId> @event)
    {
        _events.Enqueue(@event);

        Apply(@event);

        Version++;
    }

    /// <summary>
    ///     Applies the event
    /// </summary>
    /// <param name="event">DomainEvent object</param>
    protected abstract void Apply(IDomainEvent<TAggregateId> @event);

    #region Factory

    private static readonly Lazy<ConstructorInfo> LazyConstructor = new(
        () =>
        {
            var aggregateType = typeof(TAggregate);

            var constructor = aggregateType.GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null,
                Type.EmptyTypes,
                Array.Empty<ParameterModifier>());

            return constructor!;
        });

    /// <summary>
    ///     Creates an aggregate with factory design pattern
    /// </summary>
    /// <param name="events">Collection of domain events</param>
    /// <returns>The rehydrated aggregate object</returns>
    /// <exception cref="ArgumentException">Throws argument exception when there are no events</exception>
    public static TAggregate Create(IEnumerable<IDomainEvent<TAggregateId>> events)
    {
        List<IDomainEvent<TAggregateId>> enumerable = events.ToList();

        if (!enumerable.Any())
            throw new ArgumentException("Aggregate Events are non existent or empty", nameof(events));

        var constructor = LazyConstructor.Value;
        var result      = (TAggregate)constructor.Invoke(Array.Empty<object>());

        if (result is Aggregate<TAggregate, TAggregateId> aggregate)
            foreach (IDomainEvent<TAggregateId> @event in enumerable)
                aggregate.AddEvent(@event);

        result.ClearEvents();

        return result;
    }

    #endregion
}

