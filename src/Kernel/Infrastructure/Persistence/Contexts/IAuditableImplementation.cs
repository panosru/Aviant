namespace Aviant.Infrastructure.Persistence.Contexts;

using System.Linq.Expressions;
using System.Reflection;
using Application.Identity;
using Application.Persistence;
using Core.Entities;
using Core.Services;
using Core.Timing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

internal interface IAuditableImplementation<TDbContext>
    where TDbContext : class, IDbContextWrite
{
    public MethodInfo? ConfigureGlobalFiltersMethodInfo => typeof(TDbContext)
       .GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);


    private static ICurrentUserService CurrentUserService =>
        ServiceLocator.ServiceContainer.GetService<ICurrentUserService>(
            typeof(ICurrentUserService));

    #region Configure Global Filters

    private void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
        where TEntity : class
    {
        if (!ShouldFilterEntity<TEntity>(entityType))
            return;

        Expression<Func<TEntity, bool>>? filterExpression = CreateFilterExpression<TEntity>();

        if (filterExpression is not null)
            modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
    }

    protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
        where TEntity : class => typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity));

    protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>()
        where TEntity : class
    {
        Expression<Func<TEntity, bool>>? expression = null;

        if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
        {
            Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete)e).IsDeleted;
            expression = softDeleteFilter;
        }

        return expression;
    }

    #endregion


    #region Configure Audit Properties

    public virtual void SetCreationAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is not IHasCreationTime hasCreationTimeEntity)
            return;

        if (hasCreationTimeEntity.Created == default)
            hasCreationTimeEntity.Created = Clock.Now;

        if (entry.Entity is not ICreationAudited creationAuditedEntity)
            return;

        if (creationAuditedEntity.CreatedBy != Guid.Empty)
            //CreatedUserId is already set
            return;

        creationAuditedEntity.CreatedBy = CurrentUserService.UserId;
    }

    public virtual void SetModificationAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is not IHasModificationTime hasModificationTimeEntity)
            return;

        hasModificationTimeEntity.LastModified = Clock.Now;

        if (entry.Entity is not IModificationAudited modificationAuditedEntity)
            return;

        if (modificationAuditedEntity.LastModifiedBy == CurrentUserService.UserId)
            //LastModifiedUserId is same as current user id
            return;

        modificationAuditedEntity.LastModifiedBy = CurrentUserService.UserId;
    }

    public void SetDeletionAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is not IHasDeletionTime hasDeletionTimeEntity)
            return;

        hasDeletionTimeEntity.Deleted ??= Clock.Now;

        if (entry.Entity is not IDeletionAudited deletionAuditedEntity)
            return;

        deletionAuditedEntity.DeletedBy = CurrentUserService.UserId;
        deletionAuditedEntity.Deleted   = Clock.Now;
    }

    public virtual void CancelDeletionForSoftDelete(EntityEntry entry)
    {
        if (entry.Entity is not ISoftDelete)
            return;

        entry.Reload();
        entry.State                           = EntityState.Modified;
        ((ISoftDelete)entry.Entity).IsDeleted = true;
    }

    #endregion
}
