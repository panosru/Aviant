namespace Aviant.Infrastructure.Identity.Persistence.Contexts;

using Application.Identity;
using Application.Persistence;
using Core.Identity.Entities;
using Core.Services;
using Core.Timing;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public interface IAuditableImplementation<TDbContext>
    : Infrastructure.Persistence.Contexts.IAuditableImplementation<TDbContext>
    where TDbContext : class, IDbContextWrite
{
    private static ICurrentUserService CurrentUserService =>
        ServiceLocator.ServiceContainer.GetService<ICurrentUserService>(
            typeof(ICurrentUserService));

    private Infrastructure.Persistence.Contexts.IAuditableImplementation<TDbContext> AuditableImplementation =>
        this;

    #region Configure Audit Properties

    public new virtual void SetCreationAuditProperties(EntityEntry entry)
    {
        AuditableImplementation.SetCreationAuditProperties(entry);

        if (entry.Entity is not ICreationAudited creationAuditedEntity)
            return;

        if (creationAuditedEntity.CreatedBy != Guid.Empty)
            //CreatedUserId is already set
            return;

        creationAuditedEntity.CreatedBy = CurrentUserService.UserId;
    }

    public new virtual void SetModificationAuditProperties(EntityEntry entry)
    {
        AuditableImplementation.SetModificationAuditProperties(entry);

        if (entry.Entity is not IModificationAudited modificationAuditedEntity)
            return;

        if (modificationAuditedEntity.LastModifiedBy == CurrentUserService.UserId)
            //LastModifiedUserId is same as current user id
            return;

        modificationAuditedEntity.LastModifiedBy = CurrentUserService.UserId;
    }

    public new void SetDeletionAuditProperties(EntityEntry entry)
    {
        AuditableImplementation.SetDeletionAuditProperties(entry);

        if (entry.Entity is not IDeletionAudited deletionAuditedEntity)
            return;

        deletionAuditedEntity.DeletedBy = CurrentUserService.UserId;
        deletionAuditedEntity.Deleted   = Clock.Now;
    }

    #endregion
}
