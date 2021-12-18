namespace Aviant.DDD.Application.Persistence;

using Core.Aggregates;

/// <summary>
///     Unit of Work Interface
/// </summary>
// ReSharper disable once UnusedTypeParameter
public interface IUnitOfWork<TDbContext>
    where TDbContext : IDbContextWrite
{
    /// <summary>
    ///     Commit changes to database persistence
    /// </summary>
    /// <returns>Integer representing affected rows</returns>
    public Task<int> CommitAsync(CancellationToken cancellationToken = default);
}

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
