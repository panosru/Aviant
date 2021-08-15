// ReSharper disable InvalidXmlDocComment

namespace Aviant.DDD.Core.Aggregates
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using DomainEvents;
    using Entities;

    /// <inheritdoc cref="Aviant.DDD.Core.Entities.Entity&lt;TKey&gt;" />
    /// <inheritdoc cref="Aviant.DDD.Core.Aggregates.IAggregate&lt;out TAggregateId&gt;" />
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
        private readonly Queue<IDomainEvent<TAggregateId>> _events = new Queue<IDomainEvent<TAggregateId>>();

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

            if (events is null
             || !enumerable.Any())
                throw new ArgumentException("Aggregate Events are non existent or empty", nameof(events));

            var constructor = LazyConstructor.Value;
            var result      = (TAggregate)constructor.Invoke(Array.Empty<object>());

            if (result is Aggregate<TAggregate, TAggregateId> aggregate)
                foreach (var @event in enumerable)
                    aggregate.AddEvent(@event);

            result.ClearEvents();

            return result;
        }

        #endregion
    }
}