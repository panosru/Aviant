namespace Aviant.DDD.Application.Persistence;

public interface IDbContextWrite : IDisposable
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
