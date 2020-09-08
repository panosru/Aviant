namespace Aviant.DDD.Domain.Aggregates
{
    using System.Collections.Generic;
    using Entities;
    using Events;

    public interface IAggregate<out TAggregateId> : IEntity<TAggregateId>
        where TAggregateId : IAggregateId
    {
        long Version { get; }

        IReadOnlyCollection<IEvent<TAggregateId>> Events { get; }

        void ClearEvents();
    }
}