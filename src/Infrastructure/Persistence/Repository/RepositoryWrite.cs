namespace Aviant.DDD.Infrastructure.Persistence.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Application.Identity;
    using Contexts;
    using Core.Entities;
    using Core.Persistence;
    using Microsoft.EntityFrameworkCore;

    public abstract class RepositoryWriteImplementation<TDbContext, TEntity, TPrimaryKey>
        : IRepositoryWrite<TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TEntity : Entity<TPrimaryKey>
    {
        protected RepositoryWriteImplementation(TDbContext dbContext) => DbContext = dbContext;

        private TDbContext DbContext { get; }

        private DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        #region IRepositoryWrite<TEntity,TPrimaryKey> Members

        public virtual async Task AddAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default)
        {
            // First validate entity's rules
            await entity.ValidateAsync(cancellationToken)
               .ConfigureAwait(false);

            await DbSet.AddAsync(entity, cancellationToken)
               .ConfigureAwait(false);
        }

        public virtual Task UpdateAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default)
        {
            // First validate entity's rules
            entity.ValidateAsync(cancellationToken);

            DbContext.Entry(entity).State = EntityState.Modified;

            return Task.CompletedTask;
        }

        public virtual Task DeleteAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default)
        {
            // First validate entity's rules
            entity.ValidateAsync(cancellationToken);

            DbContext.Entry(entity).State = EntityState.Deleted;

            return Task.CompletedTask;
        }

        public virtual Task DeleteWhereAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default)
        {
            IEnumerable<TEntity> entities = DbSet.Where(predicate);

            foreach (var entity in entities) DbContext.Entry(entity).State = EntityState.Deleted;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                DbContext.Dispose();
        }

        ~RepositoryWriteImplementation()
        {
            Dispose(false);
        }
    }


    public abstract class RepositoryWrite<TDbContext, TEntity, TPrimaryKey>
        : RepositoryWriteImplementation<TDbContext, TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TDbContext : DbContextWrite<TDbContext>
    {
        protected RepositoryWrite(TDbContext context)
            : base(context)
        { }
    }

    public abstract class RepositoryWrite<TDbContext, TApplicationUser, TApplicationRole, TEntity, TPrimaryKey>
        : RepositoryWriteImplementation<TDbContext, TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TApplicationUser : ApplicationUser
        where TApplicationRole : ApplicationRole
        where TDbContext : AuthorizationDbContextWrite<TDbContext, TApplicationUser, TApplicationRole>
    {
        protected RepositoryWrite(TDbContext context)
            : base(context)
        { }
    }
}