namespace Aviant.DDD.Infrastructure.Persistence.Repository
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Application.Identity;
    using Contexts;
    using Domain.Entities;
    using Domain.Persistence;
    using Microsoft.EntityFrameworkCore;

    public abstract class RepositoryReadOnly<TDbContext, TApplicationUser, TApplicationRole, TEntity, TPrimaryKey>
        : IRepositoryRead<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TApplicationUser : ApplicationUser
        where TApplicationRole : ApplicationRole
        where TDbContext : ApplicationDbContextReadOnly<TApplicationUser, TApplicationRole>
    {
        private readonly TDbContext _dbContext;

        private readonly DbSet<TEntity> _dbSet;

        protected RepositoryReadOnly(TDbContext context)
        {
            _dbContext = context;
            _dbSet     = _dbContext.Set<TEntity>();
        }

        #region IRepositoryRead<TEntity,TPrimaryKey> Members

        public IQueryable<TEntity> GetAll()
        {
            IQueryable<TEntity> query = _dbSet;

            return query;
        }

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();
            BindIncludeProperties(query, includeProperties);
            includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query;
        }

        public async Task<List<TEntity>> GetAllList() => await GetAll().ToListAsync();

        public Task<List<TEntity>> GetAllListIncluding(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();
            includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.ToListAsync();
        }

        public ValueTask<TEntity> Find(TPrimaryKey id) => _dbSet.FindAsync(id);

        public Task<TEntity> GetFirst(TPrimaryKey id) =>
            GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));

        public Task<TEntity> GetFirstIncluding(
            TPrimaryKey                                id,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public Task<TEntity> GetFirst(Expression<Func<TEntity, bool>> predicate) =>
            GetAll().FirstOrDefaultAsync(predicate);

        public Task<TEntity> GetFirstIncluding(
            Expression<Func<TEntity, bool>>            predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.FirstOrDefaultAsync(predicate);
        }

        public Task<TEntity> GetSingle(TPrimaryKey id) =>
            GetAll().SingleOrDefaultAsync(CreateEqualityExpressionForId(id));

        public Task<TEntity> GetSingleIncluding(
            TPrimaryKey                                id,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.SingleOrDefaultAsync(CreateEqualityExpressionForId(id));
        }

        public Task<TEntity> GetSingle(Expression<Func<TEntity, bool>> predicate) =>
            GetAll().SingleOrDefaultAsync(predicate);

        public Task<TEntity> GetSingleIncluding(
            Expression<Func<TEntity, bool>>            predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.SingleOrDefaultAsync(predicate);
        }

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate) => GetAll().Where(predicate);

        public IQueryable<TEntity> FindByIncluding(
            Expression<Func<TEntity, bool>>            predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = GetAll();
            query = includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));

            return query.Where(predicate);
        }

        public Task<bool> Any(Expression<Func<TEntity, bool>> predicate) => _dbSet.AnyAsync(predicate);

        public Task<bool> All(Expression<Func<TEntity, bool>> predicate) => _dbSet.AllAsync(predicate);

        public async Task<int> Count() => await _dbSet.CountAsync();

        public Task<int> Count(Expression<Func<TEntity, bool>> predicate) => _dbSet.CountAsync(predicate);

        public void Dispose()
        {
            _dbContext.Dispose();
        }

        #endregion

        private static void BindIncludeProperties(
            IQueryable<TEntity>                            query,
            IEnumerable<Expression<Func<TEntity, object>>> includeProperties)
        {
            includeProperties.Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
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
}