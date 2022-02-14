namespace Aviant.Infrastructure.Persistence.Contexts;

using System.Reflection;
using Application.Persistence;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public interface IDbContextWriteImplementation<TDbContext>
    where TDbContext : class, IDbContextWrite
{
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
                    auditableImplementation.CancelDeletionForSoftDelete(entry);
                    auditableImplementation.SetDeletionAuditProperties(entry);
                    break;

                case EntityState.Detached:
                    break;

                case EntityState.Unchanged:
                    break;

                default:
                    throw new ArgumentOutOfRangeException(typeof(EntityState).FullName);
            }
    }

    public void OnPreBaseModelCreating(
        ModelBuilder      modelBuilder,
        HashSet<Assembly> configurationAssemblies)
    {
        // By default add the assembly of the derived DbContext object
        // so that if the entity configuration is in the same assembly
        // as the derived DbContext object, then you don't have to use
        // AddConfigurationAssemblyFromEntity method to specify entity
        // configuration assemblies
        configurationAssemblies.Add(GetType().Assembly);

        foreach (var assembly in configurationAssemblies)
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
    }

    public void OnPostBaseModelCreating(
        ModelBuilder                         modelBuilder,
        IAuditableImplementation<TDbContext> auditableImplementation)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            auditableImplementation.ConfigureGlobalFiltersMethodInfo?
               .MakeGenericMethod(entityType.ClrType)
               .Invoke(this, new object[] { modelBuilder, entityType });
    }
}
