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

    public abstract class RepositoryReadImplementation<TDbContext, TEntity, TPrimaryKey>
        : IRepositoryRead<TEntity, TPrimaryKey>
        where TDbContext : DbContext
        where TEntity : Entity<TPrimaryKey>
    {
        protected RepositoryReadImplementation(TDbContext dbContext) => DbContext = dbContext;

        private TDbContext DbContext { get; }

        private DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        #region IRepositoryRead<TEntity,TPrimaryKey> Members

        public virtual IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = DbSet;

            return query;
        }

        public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();
            BindIncludeProperties(query, includeProperties);

            includeProperties.Aggregate(
                query,
                (current, includeProperty) =>
                    current.Include(includeProperty));

            return query;
        }

        public virtual async Task<List<TEntity>> GetAllList(CancellationToken cancellationToken = default) =>
            await GetAll()
               .ToListAsync(cancellationToken: cancellationToken)
               .ConfigureAwait(false);

        public virtual Task<List<TEntity>> GetAllListIncluding(
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();

            includeProperties.Aggregate(
                query,
                (current, includeProperty) =>
                    current.Include(includeProperty));

            return query.ToListAsync(cancellationToken: cancellationToken);
        }

        public virtual ValueTask<TEntity> Find(
            TPrimaryKey       id,
            CancellationToken cancellationToken = default) =>
            DbSet.FindAsync(new object[] { id! }, cancellationToken);

        public virtual Task<TEntity> GetFirst(
            TPrimaryKey       id,
            CancellationToken cancellationToken = default) =>
            GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id), cancellationToken);

        public virtual Task<TEntity> GetFirstIncluding(
            TPrimaryKey                                id,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();

            query = includeProperties.Aggregate(
                query,
                (current, includeProperty) =>
                    current.Include(includeProperty));

            return query.FirstOrDefaultAsync(CreateEqualityExpressionForId(id), cancellationToken);
        }

        public virtual Task<TEntity> GetFirst(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default) =>
            GetAll().FirstOrDefaultAsync(predicate, cancellationToken);

        public virtual Task<TEntity> GetFirstIncluding(
            Expression<Func<TEntity, bool>>            predicate,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();

            query = includeProperties.Aggregate(
                query,
                (current, includeProperty) =>
                    current.Include(includeProperty));

            return query.FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual Task<TEntity> GetSingle(
            TPrimaryKey       id,
            CancellationToken cancellationToken = default) =>
            GetAll().SingleOrDefaultAsync(CreateEqualityExpressionForId(id), cancellationToken);

        public virtual Task<TEntity> GetSingleIncluding(
            TPrimaryKey                                id,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();

            query = includeProperties.Aggregate(
                query,
                (current, includeProperty) =>
                    current.Include(includeProperty));

            return query.SingleOrDefaultAsync(CreateEqualityExpressionForId(id), cancellationToken);
        }

        public virtual Task<TEntity> GetSingle(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default) =>
            GetAll().SingleOrDefaultAsync(predicate, cancellationToken);

        public virtual Task<TEntity> GetSingleIncluding(
            Expression<Func<TEntity, bool>>            predicate,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();

            query = includeProperties.Aggregate(
                query,
                (current, includeProperty) =>
                    current.Include(includeProperty));

            return query.SingleOrDefaultAsync(predicate, cancellationToken);
        }

        public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate) =>
            GetAll().Where(predicate);

        public virtual IQueryable<TEntity> FindByIncluding(
            Expression<Func<TEntity, bool>>            predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.Where(predicate);
        }

        public virtual Task<bool> Any(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default) =>
            DbSet.AnyAsync(predicate, cancellationToken);

        public virtual Task<bool> All(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default) =>
            DbSet.AllAsync(predicate, cancellationToken);

        public virtual async Task<int> Count(CancellationToken cancellationToken = default) =>
            await DbSet.CountAsync(cancellationToken: cancellationToken)
               .ConfigureAwait(false);

        public virtual Task<int> Count(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default) =>
            DbSet.CountAsync(predicate, cancellationToken);

        public void Dispose()
        {
            DbContext.Dispose();
        }

        #endregion

        private static void BindIncludeProperties(
            IQueryable<TEntity>                            query,
            IEnumerable<Expression<Func<TEntity, object>>> includeProperties)
        {
            includeProperties.Aggregate(
                query,
                (current, includeProperty) =>
                    current.Include(includeProperty));
        }

        private static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var lambdaBody = Expression.Equal(
                Expression.PropertyOrField(lambdaParam, "Id"),
                Expression.Constant(id, typeof(TPrimaryKey))
            );

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
    }


    public abstract class RepositoryRead<TDbContext, TEntity, TPrimaryKey>
        : RepositoryReadImplementation<TDbContext, TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TDbContext : DbContextRead
    {
        protected RepositoryRead(TDbContext context)
            : base(context)
        { }
    }

    public abstract class RepositoryRead<TDbContext, TApplicationUser, TApplicationRole, TEntity, TPrimaryKey>
        : RepositoryReadImplementation<TDbContext, TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TApplicationUser : ApplicationUser
        where TApplicationRole : ApplicationRole
        where TDbContext : AuthorizationDbContextRead<TApplicationUser, TApplicationRole>
    {
        protected RepositoryRead(TDbContext context)
            : base(context)
        { }
    }
}