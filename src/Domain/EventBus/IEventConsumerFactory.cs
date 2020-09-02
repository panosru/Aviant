namespace Aviant.DDD.Domain.EventBus
{
    using Aggregates;

    public interface IEventConsumerFactory
    {
        IEventConsumer Build<TAggregateRoot, TAggregateId, TDeserializer>()
            where TAggregateRoot : IAggregateRoot<TAggregateId>
            where TAggregateId : IAggregateId;
    }
}