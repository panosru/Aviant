namespace Aviant.DDD.Domain.Aggregates
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using System.Reflection;
    using Entities;
    using Events;

    public abstract class AggregateRoot<TAggregateRoot, TKey> : Entity<TKey>, IAggregateRoot<TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {
        private readonly Queue<IEvent<TKey>> _events = new Queue<IEvent<TKey>>();

        protected AggregateRoot()
        {
        }

        protected AggregateRoot(TKey id)
            : base(id)
        {
        }

        public IReadOnlyCollection<IEvent<TKey>> Events => _events.ToImmutableArray();

        public long Version { get; private set; }

        public void ClearEvents()
        {
            _events.Clear();
        }

        public void AddEvent(IEvent<TKey> @event)
        {
            _events.Enqueue(@event);

            Apply(@event);

            Version++;
        }

        protected abstract void Apply(IEvent<TKey> @event);

        #region Factory

        private static readonly Lazy<ConstructorInfo> LazyConstructor;

        static AggregateRoot()
        {
            LazyConstructor = new Lazy<ConstructorInfo>(
                () =>
                {
                    var aggregateType = typeof(TAggregateRoot);
                    var constructor = aggregateType.GetConstructor(
                        BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                        null,
                        new Type[0],
                        new ParameterModifier[0]);

                    return constructor!;
                });
        }

        public static TAggregateRoot Create(IEnumerable<IEvent<TKey>> events)
        {
            if (null == events || !events.Any())
                throw new ArgumentException(nameof(events));

            var constructor = LazyConstructor.Value;
            var result = (TAggregateRoot) constructor.Invoke(new object[0]);

            if (result is AggregateRoot<TAggregateRoot, TKey> aggregate)
                foreach (var @event in events)
                    aggregate.AddEvent(@event);

            result.ClearEvents();

            return result;
        }

        #endregion
    }
}