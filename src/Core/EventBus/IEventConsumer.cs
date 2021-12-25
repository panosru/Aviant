namespace Aviant.DDD.Core.EventBus;

using System.Diagnostics.CodeAnalysis;
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

    public event ExceptionThrownHandler ExceptionThrown;
}

public delegate Task EventReceivedHandlerAsync<in TAggregateId>(
    object                     sender,
    IDomainEvent<TAggregateId> @event,
    CancellationToken          cancellationToken = default)
    where TAggregateId : IAggregateId;

public delegate void ExceptionThrownHandler(object sender, Exception exception);
