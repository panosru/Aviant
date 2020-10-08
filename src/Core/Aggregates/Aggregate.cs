namespace Aviant.DDD.Core.Aggregates
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using DomainEvents;
    using Entities;

    public abstract class Aggregate<TAggregate, TAggregateId>
        : Entity<TAggregateId>, IAggregate<TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        private readonly Queue<IDomainEvent<TAggregateId>> _events = new Queue<IDomainEvent<TAggregateId>>();

        protected Aggregate()
        { }

        protected Aggregate(TAggregateId aggregateId)
            : base(aggregateId)
        { }

        #region IAggregate<TAggregateId> Members

        public IReadOnlyCollection<IDomainEvent<TAggregateId>> Events => _events.ToImmutableArray();

        public long Version { get; private set; }

        public void ClearEvents()
        {
            _events.Clear();
        }

        #endregion

        protected void AddEvent(IDomainEvent<TAggregateId> @event)
        {
            _events.Enqueue(@event);

            Apply(@event);

            Version++;
        }

        protected abstract void Apply(IDomainEvent<TAggregateId> @event);

        #region Factory

        private static readonly Lazy<ConstructorInfo> LazyConstructor = new Lazy<ConstructorInfo>(
            () =>
            {
                var aggregateType = typeof(TAggregate);

                var constructor = aggregateType.GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                    null,
                    new Type[0],
                    new ParameterModifier[0]);

                return constructor!;
            });

        public static TAggregate Create(IEnumerable<IDomainEvent<TAggregateId>> events)
        {
            List<IDomainEvent<TAggregateId>> enumerable = events.ToList();

            if (null == events
             || !enumerable.Any())
                throw new ArgumentException(nameof(events));

            var constructor = LazyConstructor.Value;
            var result      = (TAggregate) constructor.Invoke(new object[0]);

            if (result is Aggregate<TAggregate, TAggregateId> aggregate)
                foreach (var @event in enumerable)
                    aggregate.AddEvent(@event);

            result.ClearEvents();

            return result;
        }

        #endregion
    }
}