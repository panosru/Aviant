namespace Aviant.Infrastructure.Identity.Persistence.Contexts;

using System.Reflection;
using Application.Identity;
using Application.Persistence;
using Core.Entities;
using Duende.IdentityServer.EntityFramework.Options;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

public abstract class AuthorizationDbContextWrite<TDbContext, TApplicationUser, TApplicationRole>
    : ApiAuthorizationDbContext<TApplicationUser, TApplicationRole, Guid>,
      IDbContextWrite,
      IAuditableImplementation<TDbContext>,
      IDbContextWriteImplementation<TDbContext>
    where TDbContext : class, IDbContextWrite
    where TApplicationUser : ApplicationUser
    where TApplicationRole : ApplicationRole
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly HashSet<Assembly> ConfigurationAssemblies = new();

    private readonly IDbContextWriteImplementation<TDbContext> _writeImplementation;

    protected AuthorizationDbContextWrite(
        DbContextOptions                  options,
        IOptions<OperationalStoreOptions> operationalStoreOptions)
        : base(options, operationalStoreOptions)
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
