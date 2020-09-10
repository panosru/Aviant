namespace Aviant.DDD.Infrastructure.Persistence.Contexts
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Identity;
    using Application.Persistance;
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
        private readonly IAuditableImplementation<TDbContext> _trait;

        protected AuthorizationDbContextWrite(
            DbContextOptions                  options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
            _trait = this;

            TrackerSettings();
        }

        #region IAuthorizationDbContextWrite Members

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

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