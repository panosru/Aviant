using Aviant.Core.EventSourcing.Aggregates;

namespace Aviant.Core.EventSourcing.EventBus;

public interface IEventProducer<in TAggregate, in TAggregateId> : IDisposable
    where TAggregate : IAggregate<TAggregateId>
    where TAggregateId : IAggregateId
{
    public Task DispatchAsync(
        TAggregate        aggregate,
        CancellationToken cancellationToken = default);
}
