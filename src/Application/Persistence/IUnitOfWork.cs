namespace Aviant.Application.Persistence;

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
