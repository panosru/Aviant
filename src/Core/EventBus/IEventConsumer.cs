namespace Aviant.DDD.Core.EventBus
{
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Aggregates;
    using DomainEvents;

    public interface IEventConsumer
    {
        public Task ConsumeAsync(CancellationToken cancellationToken);
    }

    [SuppressMessage("ReSharper", "UnusedTypeParameter")]
    public interface IEventConsumer<TAggregate, out TAggregateId, TDeserializer>
        : IEventConsumer
        where TAggregate : IAggregate<TAggregateId>
        where TAggregateId : IAggregateId
    {
        public event EventReceivedHandlerAsync<TAggregateId> EventReceived;
    }

    public delegate Task EventReceivedHandlerAsync<in TAggregateId>(
        object                     sender,
        IDomainEvent<TAggregateId> @event,
        CancellationToken          cancellationToken = default)
        where TAggregateId : IAggregateId;
}