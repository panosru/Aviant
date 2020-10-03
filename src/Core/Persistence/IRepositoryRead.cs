namespace Aviant.DDD.Core.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;

    public interface IRepositoryRead<TEntity, in TPrimaryKey> : IDisposable
        where TEntity : Entity<TPrimaryKey>
    {
        public IQueryable<TEntity> GetAll();

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] includeProperties);

        public Task<List<TEntity>> GetAllListAsync(CancellationToken cancellationToken = default);

        public Task<List<TEntity>> GetAllListIncludingAsync(
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public ValueTask<TEntity> FindAsync(
            TPrimaryKey       id,
            CancellationToken cancellationToken = default);

        public Task<TEntity> GetFirstAsync(
            TPrimaryKey       id,
            CancellationToken cancellationToken = default);

        public Task<TEntity> GetFirstAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);

        public Task<TEntity> GetFirstIncludingAsync(
            TPrimaryKey                                id,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public Task<TEntity> GetFirstIncludingAsync(
            Expression<Func<TEntity, bool>>            predicate,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public Task<TEntity> GetSingleAsync(
            TPrimaryKey       id,
            CancellationToken cancellationToken = default);

        public Task<TEntity> GetSingleAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);

        public Task<TEntity> GetSingleIncludingAsync(
            TPrimaryKey                                id,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public Task<TEntity> GetSingleIncludingAsync(
            Expression<Func<TEntity, bool>>            predicate,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        public IQueryable<TEntity> FindByIncluding(
            Expression<Func<TEntity, bool>>            predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public Task<bool> AnyAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);

        public Task<bool> AllAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);

        public Task<int> CountAsync(CancellationToken cancellationToken = default);

        public Task<int> CountAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);
    }
}