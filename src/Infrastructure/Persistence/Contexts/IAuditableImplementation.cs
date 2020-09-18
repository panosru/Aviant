namespace Aviant.DDD.Infrastructure.Persistence.Contexts
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;
    using Application.Identity;
    using Application.Persistance;
    using Application.Services;
    using Core.Entities;
    using Core.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.ChangeTracking;
    using Microsoft.EntityFrameworkCore.Metadata;

    public interface IAuditableImplementation<TDbContext>
        where TDbContext : class, IDbContextWrite
    {
        public MethodInfo? ConfigureGlobalFiltersMethodInfo => typeof(TDbContext)
           .GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);


        private static ICurrentUserService CurrentUserService =>
            ServiceLocator.ServiceContainer.GetService<ICurrentUserService>(
                typeof(ICurrentUserService));

        private static IDateTimeService DateTimeService =>
            ServiceLocator.ServiceContainer.GetService<IDateTimeService>(
                typeof(IDateTimeService));

        #region Configure Global Filters

        private void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
            where TEntity : class
        {
            if (!ShouldFilterEntity<TEntity>(entityType))
                return;

            Expression<Func<TEntity, bool>>? filterExpression = CreateFilterExpression<TEntity>();
            if (filterExpression != null) modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
        }

        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType)
            where TEntity : class => typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity));

        protected virtual Expression<Func<TEntity, bool>>? CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>>? expression = null;

            if (typeof(ISoftDelete).IsAssignableFrom(typeof(TEntity)))
            {
                Expression<Func<TEntity, bool>> softDeleteFilter = e => !((ISoftDelete) e).IsDeleted;
                expression = softDeleteFilter;
            }

            return expression;
        }

        #endregion


        #region Configure Audit Properties

        public virtual void SetCreationAuditProperties(EntityEntry entry)
        {
            if (!(entry.Entity is IHasCreationTime hasCreationTimeEntity)) return;

            if (hasCreationTimeEntity.Created == default) hasCreationTimeEntity.Created = DateTimeService.Now(true);

            if (!(entry.Entity is ICreationAudited creationAuditedEntity)) return;

            if (creationAuditedEntity.CreatedBy != Guid.Empty)
                //CreatedUserId is already set
                return;

            creationAuditedEntity.CreatedBy = CurrentUserService.UserId;
        }

        public virtual void SetModificationAuditProperties(EntityEntry entry)
        {
            if (!(entry.Entity is IHasModificationTime hasModificationTimeEntity)) return;

            hasModificationTimeEntity.LastModified = DateTimeService.Now(true);

            if (!(entry.Entity is IModificationAudited modificationAuditedEntity)) return;

            if (modificationAuditedEntity.LastModifiedBy == CurrentUserService.UserId)
                //LastModifiedUserId is same as current user id
                return;

            modificationAuditedEntity.LastModifiedBy = CurrentUserService.UserId;
        }

        public void SetDeletionAuditProperties(EntityEntry entry)
        {
            if (!(entry.Entity is IHasDeletionTime hasDeletionTimeEntity)) return;

            hasDeletionTimeEntity.Deleted ??= DateTimeService.Now(true);

            if (!(entry.Entity is IDeletionAudited deletionAuditedEntity)) return;

            deletionAuditedEntity.DeletedBy = CurrentUserService.UserId;
            deletionAuditedEntity.Deleted   = DateTimeService.Now(true);
        }

        public virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete)) return;

            entry.Reload();
            entry.State                            = EntityState.Modified;
            ((ISoftDelete) entry.Entity).IsDeleted = true;
        }

        #endregion
    }
}