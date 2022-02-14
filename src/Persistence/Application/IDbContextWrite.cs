namespace Aviant.Application.Persistence;

public interface IDbContextWrite : IDisposable
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
