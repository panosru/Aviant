namespace Aviant.DDD.Infrastructure.Persistence.Contexts
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Identity;
    using Application.Persistance;
    using Configurations;
    using Domain.Entities;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.Extensions.Options;

    public abstract class AuthorizationDbContextWrite<TDbContext, TApplicationUser, TApplicationRole>
        : ApiAuthorizationDbContext<TApplicationUser, TApplicationRole, Guid>,
          IDbContextWrite,
          IAuditableImplementation<TDbContext>
        where TDbContext : class, IDbContextWrite
        where TApplicationUser : ApplicationUser
        where TApplicationRole : ApplicationRole
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly HashSet<Assembly> ConfigurationAssemblies = new HashSet<Assembly>();

        private readonly IAuditableImplementation<TDbContext> _trait;

        protected AuthorizationDbContextWrite(
            DbContextOptions                  options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
            _trait = this;

            TrackerSettings();
        }

        #region IDbContextWrite Members

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (EntityEntry<IAuditedEntity> entry in ChangeTracker.Entries<IAuditedEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        _trait.SetCreationAuditProperties(entry);
                        break;

                    case EntityState.Modified:
                        _trait.SetModificationAuditProperties(entry);
                        break;

                    case EntityState.Deleted:
                        _trait.CancelDeletionForSoftDelete(entry);
                        _trait.SetDeletionAuditProperties(entry);
                        break;

                    case EntityState.Detached:
                        break;

                    case EntityState.Unchanged:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }

            var result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

            return result;
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
            // By default add the assembly of the dervided DbContext object
            // so that if the entity configuration is in the same assembly
            // as the derived DbContext object, then you don't have to use
            // AddConfigurationAssemblyFromEntity method to specify entity
            // configuration assemblies
            ConfigurationAssemblies.Add(GetType().Assembly);

            foreach (var assembly in ConfigurationAssemblies)
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);

            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                _trait.ConfigureGlobalFiltersMethodInfo?
                   .MakeGenericMethod(entityType.ClrType)
                   .Invoke(this, new object[] { modelBuilder, entityType });
        }

        private void TrackerSettings()
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }
    }
}