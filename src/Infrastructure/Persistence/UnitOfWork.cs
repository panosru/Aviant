namespace Aviant.Foundation.Infrastructure.Persistence;

using Application.Persistence;

public sealed class UnitOfWork<TDbContext> : IUnitOfWork<TDbContext>, IDisposable
    where TDbContext : IDbContextWrite
{
    private readonly TDbContext _context;

    private bool _isDisposed;

    public UnitOfWork(TDbContext context) => _context = context;

    #region IDisposable Members

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    #region IUnitOfWork<TDbContext> Members

    public async Task<int> CommitAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken)
           .ConfigureAwait(false);

    #endregion

    ~UnitOfWork() => Dispose(false);

    private void Dispose(bool disposing)
    {
        if (!_isDisposed && disposing)
            _context.Dispose();

        _isDisposed = true;
    }
}
