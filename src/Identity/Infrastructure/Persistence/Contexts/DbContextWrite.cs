using Aviant.Application.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Aviant.Infrastructure.Identity.Persistence.Contexts;

public abstract class DbContextWrite<TDbContext>
    : Aviant.Infrastructure.Persistence.Contexts.DbContextWrite<TDbContext>,
      IAuditableImplementation<TDbContext>,
      IDbContextWriteImplementation<TDbContext>
    where TDbContext : class, IDbContextWrite
{
    /// <inheritdoc cref="WriteImplementation" />
    protected new IDbContextWriteImplementation<TDbContext> WriteImplementation => this;

    /// <inheritdoc />
    protected DbContextWrite(DbContextOptions options)
        : base(options)
    { }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        WriteImplementation.ChangeTracker(ChangeTracker, this);

        return CommitAsync(cancellationToken);
    }
}
