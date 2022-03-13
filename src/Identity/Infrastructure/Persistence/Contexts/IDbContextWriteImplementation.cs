namespace Aviant.Infrastructure.Identity.Persistence.Contexts;

using Application.Persistence;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public interface IDbContextWriteImplementation<TDbContext>
    : Infrastructure.Persistence.Contexts.IDbContextWriteImplementation<TDbContext>
    where TDbContext : class, IDbContextWrite
{
    private Infrastructure.Persistence.Contexts.IDbContextWriteImplementation<TDbContext> Parent => this;

    public void ChangeTracker(
        ChangeTracker                        trackerChange,
        IAuditableImplementation<TDbContext> auditableImplementation)
    {
        foreach (EntityEntry<IAuditedEntity> entry in trackerChange.Entries<IAuditedEntity>())
            switch (entry.State)
            {
                case EntityState.Added:
                    auditableImplementation.SetCreationAuditProperties(entry);
                    break;

                case EntityState.Modified:
                    auditableImplementation.SetModificationAuditProperties(entry);
                    break;

                case EntityState.Deleted:
                    auditableImplementation.SetDeletionAuditProperties(entry);
                    break;

                case EntityState.Detached:
                    break;

                case EntityState.Unchanged:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(typeof(EntityState).FullName);
            }

        Parent.ChangeTracker(
            trackerChange,
            auditableImplementation);
    }
}
