namespace Aviant.DDD.Domain.Aggregates
{
    using System.Collections.Generic;
    using Entities;
    using Events;

    public interface IAggregateRoot<out TKey> : IEntity<TKey>
    {
        public long Version { get; }
        
        IReadOnlyCollection<IEvent<TKey>> Events { get; }

        void ClearEvents();
    }
}