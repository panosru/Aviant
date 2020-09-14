namespace Aviant.DDD.Core.EventBus
{
    #region

    using Aggregates;

    #endregion

    public interface IEventConsumerFactory
    {
        IEventConsumer Build<TAggregate, TAggregateId, TDeserializer>()
            where TAggregate : IAggregate<TAggregateId>
            where TAggregateId : IAggregateId;
    }
}