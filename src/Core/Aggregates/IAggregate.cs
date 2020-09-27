namespace Aviant.DDD.Core.Aggregates
{
    using System.Collections.Generic;
    using Entities;
    using Events;

    public interface IAggregate<out TAggregateId> : IEntity<TAggregateId>
        where TAggregateId : IAggregateId
    {
        public long Version { get; }

        public IReadOnlyCollection<IEvent<TAggregateId>> Events { get; }

        public void ClearEvents();
    }
}