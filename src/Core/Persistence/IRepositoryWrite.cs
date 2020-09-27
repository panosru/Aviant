namespace Aviant.DDD.Core.Persistence
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;

    public interface IRepositoryWrite<TEntity, in TPrimaryKey> : IDisposable
        where TEntity : Entity<TPrimaryKey>
    {
        public Task AddAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        public Task UpdateAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        public Task DeleteAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        public Task DeleteWhereAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);
    }
}