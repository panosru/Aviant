namespace Aviant.Foundation.Infrastructure.Persistence.Contexts;

using Application.Identity;
using Application.Persistence;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public abstract class AuthorizationDbContextRead<TApplicationUser, TApplicationRole>
    : ApiAuthorizationDbContext<TApplicationUser, TApplicationRole, Guid>,
      IDbContextRead,
      IDbContextReadImplementation
    where TApplicationUser : ApplicationUser
    where TApplicationRole : ApplicationRole
{
    private readonly IDbContextReadImplementation _readImplementation;

    protected AuthorizationDbContextRead(
        DbContextOptions                  options,
        IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
    {
        // trait
        _readImplementation = this;

        TrackerSettings();
    }

    public override int SaveChanges()
    {
        IDbContextReadImplementation.ThrowWriteException();

        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        IDbContextReadImplementation.ThrowWriteException();

        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool              acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = new())
    {
        IDbContextReadImplementation.ThrowWriteException();

        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        IDbContextReadImplementation.ThrowWriteException();

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _readImplementation.OnPreBaseModelCreating(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void TrackerSettings() => IDbContextReadImplementation.TrackerSettings(ChangeTracker);
}
