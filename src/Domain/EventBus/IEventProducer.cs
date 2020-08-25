namespace Aviant.DDD.Domain.EventBus
{
    using System.Threading.Tasks;
    using Aggregates;

    public interface IEventProducer<in TAggregateRoot, in TKey>
        where TAggregateRoot : IAggregateRoot<TKey>
    {
        Task DispatchAsync(TAggregateRoot aggregateRoot);
    }
}