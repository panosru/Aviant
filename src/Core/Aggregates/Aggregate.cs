namespace Aviant.DDD.Core.Aggregates
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using Entities;
    using Events;

    public abstract class Aggregate<TAggregate, TAggregateId>
        : Entity<TAggregateId>, IAggregate<TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        private readonly Queue<IEvent<TAggregateId>> _events = new Queue<IEvent<TAggregateId>>();

        protected Aggregate()
        { }

        protected Aggregate(TAggregateId aggregateId)
            : base(aggregateId)
        { }

        #region IAggregate<TAggregateId> Members

        public IReadOnlyCollection<IEvent<TAggregateId>> Events => _events.ToImmutableArray();

        public long Version { get; private set; }

        public void ClearEvents()
        {
            _events.Clear();
        }

        #endregion

        protected void AddEvent(IEvent<TAggregateId> @event)
        {
            _events.Enqueue(@event);

            Apply(@event);

            Version++;
        }

        protected abstract void Apply(IEvent<TAggregateId> @event);

        #region Factory

        // ReSharper disable once StaticMemberInGenericType
        private static readonly Lazy<ConstructorInfo> LazyConstructor;

        static Aggregate()
        {
            LazyConstructor = new Lazy<ConstructorInfo>(
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
        }

        public static TAggregate Create(IEnumerable<IEvent<TAggregateId>> events)
        {
            List<IEvent<TAggregateId>>? enumerable = events.ToList();

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