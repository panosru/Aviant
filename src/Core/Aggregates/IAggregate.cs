namespace Aviant.DDD.Core.Aggregates
{
    #region

    using System.Collections.Generic;
    using Entities;
    using Events;

    #endregion

    public interface IAggregate<out TAggregateId> : IEntity<TAggregateId>
        where TAggregateId : IAggregateId
    {
        long Version { get; }

        IReadOnlyCollection<IEvent<TAggregateId>> Events { get; }

        void ClearEvents();
    }
}