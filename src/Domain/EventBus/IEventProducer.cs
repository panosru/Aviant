namespace Aviant.DDD.Domain.EventBus
{
    using System.Threading.Tasks;
    using Aggregates;

    public interface IEventProducer<in TAggregateRoot, in TAggregateId>
        where TAggregateRoot : IAggregateRoot<TAggregateId>
        where TAggregateId : IAggregateId
    {
        Task DispatchAsync(TAggregateRoot aggregateRoot);
    }
}