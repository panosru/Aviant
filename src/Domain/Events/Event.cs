namespace Aviant.DDD.Domain.Events
{
    using System;
    using Aggregates;

    public abstract class Event<TAggregate, TAggregateId> : IEvent<TAggregateId>
        where TAggregate : IAggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        protected Event()
        { }

        protected Event(TAggregate aggregate)
        {
            if (aggregate is null)
                throw new ArgumentNullException(nameof(aggregate));

            AggregateVersion = aggregate.Version;
            AggregateId      = aggregate.Id;
        }

        #region IEvent<TAggregateId> Members

        public long AggregateVersion { get; }

        public TAggregateId AggregateId { get; }

        #endregion
    }
}