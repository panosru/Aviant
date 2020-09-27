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
        public Task Add(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        public Task Update(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        public Task Delete(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        public Task DeleteWhere(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);
    }
}