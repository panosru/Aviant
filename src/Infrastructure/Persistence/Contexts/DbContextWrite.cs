namespace Aviant.DDD.Infrastructure.Persistence.Contexts
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Persistance;
    using Configurations;
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;

    #endregion

    public abstract class DbContextWrite<TDbContext>
        : DbContext, IDbContextWrite, IAuditableImplementation<TDbContext>
        where TDbContext : class, IDbContextWrite
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly HashSet<Assembly> ConfigurationAssemblies = new HashSet<Assembly>();

        private readonly IAuditableImplementation<TDbContext> _trait;

        protected DbContextWrite(DbContextOptions options)
            : base(options)
        {
            _trait = this;

            TrackerSettings();
        }

        #region IDbContextWrite Members

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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