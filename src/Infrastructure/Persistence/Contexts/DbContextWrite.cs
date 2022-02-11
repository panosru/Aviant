namespace Aviant.Infrastructure.Persistence.Contexts;

using System.Reflection;
using Application.Persistence;
using Configurations;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

public abstract class DbContextWrite<TDbContext>
    : DbContext,
      IDbContextWrite,
      IAuditableImplementation<TDbContext>,
      IDbContextWriteImplementation<TDbContext>
    where TDbContext : class, IDbContextWrite
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly HashSet<Assembly> ConfigurationAssemblies = new();

    private readonly IDbContextWriteImplementation<TDbContext> _writeImplementation;

    protected DbContextWrite(DbContextOptions options)
        : base(options)
    {
        // trait
        _writeImplementation = this;

        TrackerSettings();
    }

    #region IDbContextWrite Members

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        _writeImplementation.ChangeTracker(ChangeTracker, this);

        return base.SaveChangesAsync(cancellationToken);
    }

    #endregion

    public static void AddConfigurationAssemblyFromEntity<TEntity, TKey>(
        EntityConfiguration<TEntity, TKey> entityConfiguration)
        where TEntity : Entity<TKey>
    {
        ConfigurationAssemblies.Add(entityConfiguration.GetType().Assembly);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _writeImplementation.OnPreBaseModelCreating(modelBuilder, ConfigurationAssemblies);

        base.OnModelCreating(modelBuilder);

        _writeImplementation.OnPostBaseModelCreating(modelBuilder, this);
    }

    private void TrackerSettings()
    {
        ChangeTracker.LazyLoadingEnabled = false;
    }
}
