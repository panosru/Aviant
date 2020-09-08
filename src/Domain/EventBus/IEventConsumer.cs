namespace Aviant.DDD.Domain.EventBus
{
    using System.Threading;
    using System.Threading.Tasks;
    using Aggregates;
    using Events;

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