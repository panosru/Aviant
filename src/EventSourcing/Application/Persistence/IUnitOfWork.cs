using Aviant.Core.EventSourcing.Aggregates;

namespace Aviant.Application.EventSourcing.Persistence;

public interface IUnitOfWork<in TAggregate, TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    /// <summary>
    ///     Commit changes to event sourcing persistence
    /// </summary>
    /// <param name="aggregate"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public Task<bool> CommitAsync(TAggregate aggregate, CancellationToken cancellationToken = default);
}
