namespace Aviant.DDD.Domain.EventBus
{
    using System;
    using System.Threading.Tasks;
    using Aggregates;

    public interface IEventProducer<in TAggregate, in TAggregateId> : IDisposable
        where TAggregate : IAggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        Task DispatchAsync(TAggregate aggregate);
    }
}