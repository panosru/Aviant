namespace Aviant.DDD.Core.Events
{
    #region

    using System;
    using Aggregates;

    #endregion

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

        public long AggregateVersion { get; private set; }

        public TAggregateId AggregateId { get; private set; }

        #endregion
    }
}