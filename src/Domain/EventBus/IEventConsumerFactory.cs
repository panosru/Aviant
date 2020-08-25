namespace Aviant.DDD.Domain.EventBus
{
    using Aggregates;

    public interface IEventConsumerFactory
    {
        IEventConsumer Build<TAggregateRoot, TKey>()
            where TAggregateRoot : IAggregateRoot<TKey>;
    }
}