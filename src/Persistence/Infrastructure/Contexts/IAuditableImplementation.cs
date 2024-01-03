using System.Linq.Expressions;
using System.Reflection;
using Aviant.Application.Persistence;
using Aviant.Core.Entities;
using Aviant.Core.Timing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Aviant.Infrastructure.Persistence.Contexts;

public interface IAuditableImplementation<TDbContext>
    where TDbContext : class, IDbContextWrite
{
    public MethodInfo? ConfigureGlobalFiltersMethodInfo => typeof(TDbContext)
       .GetMethod(nameof(ConfigureGlobalFilters), BindingFlags.Instance | BindingFlags.NonPublic);

    #region Configure Global Filters

    protected void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType entityType)
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
    }

    public virtual void SetUpdateAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is not IHasUpdatedTime hasUpdateTimeEntity)
            return;

        hasUpdateTimeEntity.Updated = Clock.Now;
    }

    public virtual void SetDeletionAuditProperties(EntityEntry entry)
    {
        if (entry.Entity is not IHasDeletionTime hasDeletionTimeEntity)
            return;

        hasDeletionTimeEntity.Deleted ??= Clock.Now;
    }

    public virtual void CancelDeletionForSoftDelete(EntityEntry entry)
    {
        if (entry.Entity is not ISoftDelete entity)
            return;

        entry.State      = EntityState.Unchanged;
        entity.IsDeleted = true;
    }

    #endregion
}
