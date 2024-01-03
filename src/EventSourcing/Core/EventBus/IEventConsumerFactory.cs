using Aviant.Core.EventSourcing.Aggregates;

namespace Aviant.Core.EventSourcing.EventBus;

public interface IEventConsumerFactory
{
    public IEventConsumer Build<TAggregate, TAggregateId, TDeserializer>()
        where TAggregate : IAggregate<TAggregateId>
        where TAggregateId : IAggregateId;
}
