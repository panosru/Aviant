using Aviant.Application.Identity;
using Aviant.Application.Persistence;
using Aviant.Core.Identity.Entities;
using Aviant.Core.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Aviant.Infrastructure.Identity.Persistence.Contexts;

public interface IAuditableImplementation<TDbContext>
    : Infrastructure.Persistence.Contexts.IAuditableImplementation<TDbContext>
    where TDbContext : class, IDbContextWrite
{
    private static ICurrentUserService CurrentUserService =>
        ServiceLocator.ServiceContainer.GetService<ICurrentUserService>(
            typeof(ICurrentUserService));

    #region Configure Audit Properties

    public new virtual void SetCreationAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is not ICreationAudited creationAuditedEntity)
            return;

        if (creationAuditedEntity.CreatedBy != Guid.Empty)
            //CreatedUserId is already set
            return;

        creationAuditedEntity.CreatedBy = CurrentUserService.UserId;
    }

    public new virtual void SetUpdateAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is not IUpdatedAudited updateAuditedEntity)
            return;

        if (updateAuditedEntity.UpdatedBy == CurrentUserService.UserId)
            //LastModifiedUserId is same as current user id
            return;

        updateAuditedEntity.UpdatedBy = CurrentUserService.UserId;
    }

    public new void SetDeletionAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is not IDeletionAudited deletionAuditedEntity)
            return;

        deletionAuditedEntity.DeletedBy = CurrentUserService.UserId;
    }

    #endregion
}
