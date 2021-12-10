namespace Aviant.DDD.Core.Persistence;

using Aggregates;

public interface IEventsRepository<TAggregate, in TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    public Task AppendAsync(
        TAggregate        aggregate,
        CancellationToken cancellationToken = default);

    public Task<TAggregate> RehydrateAsync(
        TAggregateId      aggregateId,
        CancellationToken cancellationToken = default);
}
