namespace Aviant.DDD.Domain.EventBus
{
    #region

    using System.Threading;
    using System.Threading.Tasks;
    using Aggregates;
    using Events;

    #endregion

    public interface IEventConsumer
    {
        Task ConsumeAsync(CancellationToken cancellationToken);
    }

    public interface IEventConsumer<TAggregate, out TAggregateId, TDeserializer>
        : IEventConsumer
        where TAggregate : IAggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        event EventReceivedHandler<TAggregateId> EventReceived;
    }

    public delegate Task EventReceivedHandler<in TAggregateId>(object sender, IEvent<TAggregateId> @event)
        where TAggregateId : IAggregateId;
}