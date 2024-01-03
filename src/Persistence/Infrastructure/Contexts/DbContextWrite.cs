using System.Reflection;
using Aviant.Application.Persistence;
using Aviant.Infrastructure.Persistence.Configurations;
using Aviant.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Aviant.Infrastructure.Persistence.Contexts;

public abstract class DbContextWrite<TDbContext>
    : DbContext,
      IDbContextWrite,
      IAuditableImplementation<TDbContext>,
      IDbContextWriteImplementation<TDbContext>
    where TDbContext : class, IDbContextWrite
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly HashSet<Assembly> ConfigurationAssemblies = new();

    protected IDbContextWriteImplementation<TDbContext> WriteImplementation => this;

    protected DbContextWrite(DbContextOptions options)
        : base(options) => TrackerSettings();

    #region IDbContextWrite Members

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        WriteImplementation.ChangeTracker(ChangeTracker, this);

        return CommitAsync(cancellationToken);
    }

    protected Task<int> CommitAsync(CancellationToken cancellationToken = new()) =>
        base.SaveChangesAsync(cancellationToken);

    #endregion

    public static void AddConfigurationAssemblyFromEntity<TEntity, TKey>(
        EntityConfiguration<TEntity, TKey> entityConfiguration)
        where TEntity : Entity<TKey>
    {
        ConfigurationAssemblies.Add(entityConfiguration.GetType().Assembly);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        WriteImplementation.OnPreBaseModelCreating(modelBuilder, ConfigurationAssemblies);

        base.OnModelCreating(modelBuilder);

        WriteImplementation.OnPostBaseModelCreating(modelBuilder, this);
    }

    private void TrackerSettings()
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }
}
