namespace Aviant.DDD.Core.Aggregates
{
    using System.Collections.Generic;
    using DomainEvents;
    using Entities;

    public interface IAggregate<out TAggregateId> : IEntity<TAggregateId>
        where TAggregateId : IAggregateId
    {
        public long Version { get; }

        public IReadOnlyCollection<IDomainEvent<TAggregateId>> Events { get; }

        public void ClearEvents();
    }
}