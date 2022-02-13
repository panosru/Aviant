namespace Aviant.Core.EventSourcing.EventBus;

using Aggregates;

public interface IEventConsumerFactory
{
    public IEventConsumer Build<TAggregate, TAggregateId, TDeserializer>()
        where TAggregate : IAggregate<TAggregateId>
        where TAggregateId : IAggregateId;
}
