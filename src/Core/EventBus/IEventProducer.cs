namespace Aviant.Core.EventBus;

using Aggregates;

public interface IEventProducer<in TAggregate, in TAggregateId> : IDisposable
    where TAggregate : IAggregate<TAggregateId>
    where TAggregateId : IAggregateId
{
    public Task DispatchAsync(
        TAggregate        aggregate,
        CancellationToken cancellationToken = default);
}
