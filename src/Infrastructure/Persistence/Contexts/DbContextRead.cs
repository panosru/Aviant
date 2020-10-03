namespace Aviant.DDD.Infrastructure.Persistence.Contexts
{
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Persistance;
    using Microsoft.EntityFrameworkCore;

    public abstract class DbContextRead
        : DbContext,
          IDbContextRead,
          IDbContextReadImplementation
    {
        private readonly IDbContextReadImplementation _readImplementation;

        protected DbContextRead(DbContextOptions options)
            : base(options)
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
            CancellationToken cancellationToken = new CancellationToken())
        {
            IDbContextReadImplementation.ThrowWriteException();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
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
}