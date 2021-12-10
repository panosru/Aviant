namespace Aviant.DDD.Application.Persistance;

public interface IDbContextWrite : IDisposable
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
