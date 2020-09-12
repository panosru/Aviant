namespace Aviant.DDD.Infrastructure.Persistence.Contexts
{
    #region

    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Persistance;
    using Microsoft.EntityFrameworkCore;

    #endregion

    public abstract class DbContextRead
        : DbContext, IDbContextRead
    {
        protected DbContextRead(DbContextOptions options)
            : base(options)
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

        private void TrackerSettings()
        {
            ChangeTracker.LazyLoadingEnabled    = false;
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        private static void ThrowWriteException()
        {
            throw new Exception("Read-only context");
        }
    }
}