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

        public Task<List<TEntity>> GetAllList(CancellationToken cancellationToken = default);

        public Task<List<TEntity>> GetAllListIncluding(
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public ValueTask<TEntity> Find(
            TPrimaryKey       id,
            CancellationToken cancellationToken = default);

        public Task<TEntity> GetFirst(
            TPrimaryKey       id,
            CancellationToken cancellationToken = default);

        public Task<TEntity> GetFirstIncluding(
            TPrimaryKey                                id,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public Task<TEntity> GetFirst(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);

        public Task<TEntity> GetFirstIncluding(
            Expression<Func<TEntity, bool>>            predicate,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public Task<TEntity> GetSingle(
            TPrimaryKey       id,
            CancellationToken cancellationToken = default);

        public Task<TEntity> GetSingleIncluding(
            TPrimaryKey                                id,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public Task<TEntity> GetSingle(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);

        public Task<TEntity> GetSingleIncluding(
            Expression<Func<TEntity, bool>>            predicate,
            CancellationToken                          cancellationToken = default,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate);

        public IQueryable<TEntity> FindByIncluding(
            Expression<Func<TEntity, bool>>            predicate,
            params Expression<Func<TEntity, object>>[] includeProperties);

        public Task<bool> Any(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);

        public Task<bool> All(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);

        public Task<int> Count(CancellationToken cancellationToken = default);

        public Task<int> Count(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);
    }
}