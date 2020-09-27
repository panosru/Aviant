namespace Aviant.DDD.Core.EventBus
{
    using System.Threading;
    using System.Threading.Tasks;
    using Aggregates;
    using Events;

    public interface IEventConsumer
    {
        public Task ConsumeAsync(CancellationToken cancellationToken);
    }

    public interface IEventConsumer<TAggregate, out TAggregateId, TDeserializer>
        : IEventConsumer
        where TAggregate : IAggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        public event EventReceivedHandlerAsync<TAggregateId> EventReceived;
    }

    public delegate Task EventReceivedHandlerAsync<in TAggregateId>(
        object               sender,
        IEvent<TAggregateId> @event,
        CancellationToken    cancellationToken = default)
        where TAggregateId : IAggregateId;
}