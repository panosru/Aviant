namespace Aviant.EventSourcing.Core.EventBus;

using Aggregates;

public interface IEventConsumerFactory
{
    public IEventConsumer Build<TAggregate, TAggregateId, TDeserializer>()
        where TAggregate : IAggregate<TAggregateId>
        where TAggregateId : IAggregateId;
}
