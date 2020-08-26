namespace Aviant.DDD.Domain.Events
{
    using System;
    using Aggregates;

    public abstract class Event<TAggregateRoot, TKey> : IEvent<TKey>
        where TAggregateRoot : IAggregateRoot<TKey>
    {
        protected Event()
        {
        }

        protected Event(TAggregateRoot aggregateRoot)
        {
            if (aggregateRoot is null)
                throw new ArgumentNullException(nameof(aggregateRoot));

            AggregateVersion = aggregateRoot.Version;
            AggregateId = aggregateRoot.Id;
        }

        public long AggregateVersion { get; }

        public TKey AggregateId { get; }
    }
}