namespace Aviant.DDD.Core.Persistence
{
    #region

    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Entities;

    #endregion

    public interface IRepositoryWrite<TEntity, in TPrimaryKey> : IDisposable
        where TEntity : Entity<TPrimaryKey>
    {
        Task Add(TEntity entity);

        Task Update(TEntity entity);

        Task Delete(TEntity entity);

        Task DeleteWhere(Expression<Func<TEntity, bool>> predicate);
    }
}