namespace Aviant.DDD.Domain.Events
{
    using System;
    using Aggregates;

    public abstract class Event<TAggregateRoot, TAggregateId> : IEvent<TAggregateId>
        where TAggregateRoot : IAggregateRoot<TAggregateId>
        where TAggregateId : IAggregateId
    {
        protected Event()
        { }

        protected Event(TAggregateRoot aggregateRoot)
        {
            if (aggregateRoot is null)
                throw new ArgumentNullException(nameof(aggregateRoot));

            AggregateVersion = aggregateRoot.Version;
            AggregateId      = aggregateRoot.Id;
        }

    #region IEvent<TAggregateId> Members

        public long AggregateVersion { get; }

        public TAggregateId AggregateId { get; }

    #endregion
    }
}