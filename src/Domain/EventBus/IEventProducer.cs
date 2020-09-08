namespace Aviant.DDD.Domain.EventBus
{
    using System;
    using System.Threading.Tasks;
    using Aggregates;

    public interface IEventProducer<in TAggregateRoot, in TAggregateId> : IDisposable
        where TAggregateRoot : IAggregateRoot<TAggregateId>
        where TAggregateId : IAggregateId
    {
        Task DispatchAsync(TAggregateRoot aggregateRoot);
    }
}