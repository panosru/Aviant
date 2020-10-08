namespace Aviant.DDD.Core.DomainEvents
{
    using System;
    using Aggregates;

    public abstract class DomainEvent<TAggregate, TAggregateId> : IDomainEvent<TAggregateId>
        where TAggregate : IAggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        #pragma warning disable 8618
        protected DomainEvent()
        { }
        #pragma warning restore 8618

        protected DomainEvent(TAggregate aggregate)
        {
            if (aggregate is null)
                throw new ArgumentNullException(nameof(aggregate));

            AggregateVersion = aggregate.Version;
            AggregateId      = aggregate.Id;
        }

        #region IDomainEvent<TAggregateId> Members

        public long AggregateVersion { get; private set; }

        public TAggregateId AggregateId { get; private set; }

        #endregion
    }
}