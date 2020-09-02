namespace Aviant.DDD.Infrastructure.Persistence.Contexts
{
    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Identity;
    using Application.Persistance;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    public abstract class ApplicationDbContextReadOnly<TApplicationUser, TApplicationRole>
        : ApiAuthorizationDbContext<TApplicationUser, TApplicationRole, Guid>, IApplicationDbContextReadOnly
        where TApplicationUser : ApplicationUser
        where TApplicationRole : ApplicationRole
    {
        protected ApplicationDbContextReadOnly(
            DbContextOptions                  options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        {
            TrackerSettings();
        }

        public override int SaveChanges()
        {
            ThrowWriteException();

            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ThrowWriteException();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(
            bool              acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new CancellationToken())
        {
            ThrowWriteException();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            ThrowWriteException();

            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var foreignKey in modelBuilder.Model.GetEntityTypes()
                                                   .SelectMany(e => e.GetForeignKeys()))
                foreignKey.DeleteBehavior = DeleteBehavior.Restrict;

            modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

            base.OnModelCreating(modelBuilder);
        }

        protected void TrackerSettings()
        {
            ChangeTracker.LazyLoadingEnabled    = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        private void ThrowWriteException()
        {
            throw new Exception("Read-only context");
        }
    }
}