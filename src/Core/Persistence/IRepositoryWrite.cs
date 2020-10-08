namespace Aviant.DDD.Core.Persistence
{
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;
    using Entities;

    /// <inheritdoc />
    /// <summary>
    ///     This interface is implemented by all write repositories to ensure implementation of fixed methods.
    /// </summary>
    /// <typeparam name="TEntity">Main Entity type this repository works on</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity</typeparam>
    public interface IRepositoryWrite<TEntity, TPrimaryKey> : IDisposable
        where TEntity : Entity<TPrimaryKey>
    {
        #region Insert

        /// <summary>
        ///     Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        public TEntity Insert(TEntity entity);

        /// <summary>
        ///     Inserts a new entity.
        /// </summary>
        /// <param name="entity">Inserted entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public Task<TEntity> InsertAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Inserts a new entity and gets it's Id.
        ///     It may require to save current unit of work
        ///     to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        public TPrimaryKey InsertAndGetId(TEntity entity);

        /// <summary>
        ///     Inserts a new entity and gets it's Id.
        ///     It may require to save current unit of work
        ///     to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Id of the entity</returns>
        public Task<TPrimaryKey> InsertAndGetIdAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Inserts or updates given entity depending on Id's value.
        /// </summary>
        /// <param name="entity">Entity</param>
        public TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        ///     Inserts or updates given entity depending on Id's value.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public Task<TEntity> InsertOrUpdateAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Inserts or updates given entity depending on Id's value.
        ///     Also returns Id of the entity.
        ///     It may require to save current unit of work
        ///     to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>Id of the entity</returns>
        public TPrimaryKey InsertOrUpdateAndGetId(TEntity entity);

        /// <summary>
        ///     Inserts or updates given entity depending on Id's value.
        ///     Also returns Id of the entity.
        ///     It may require to save current unit of work
        ///     to be able to retrieve id.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Id of the entity</returns>
        public Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        #endregion

        #region Update

        /// <summary>
        ///     Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        public TEntity Update(TEntity entity);

        /// <summary>
        ///     Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <returns>Updated entity</returns>
        public TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);

        /// <summary>
        ///     Updates an existing entity.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public Task<TEntity> UpdateAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Updates an existing entity.
        /// </summary>
        /// <param name="id">Id of the entity</param>
        /// <param name="updateAction">Action that can be used to change values of the entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        /// <returns>Updated entity</returns>
        public Task<TEntity> UpdateAsync(
            TPrimaryKey         id,
            Func<TEntity, Task> updateAction,
            CancellationToken   cancellationToken = default);

        #endregion

        #region Delete

        /// <summary>
        ///     Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        public void Delete(TEntity entity);

        /// <summary>
        ///     Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        public void Delete(TPrimaryKey id);

        /// <summary>
        ///     Deletes many entities by function.
        ///     Notice that: All entities fits to given predicate are retrieved and deleted.
        ///     This may cause major performance problems if there are too many entities with
        ///     given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        public void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     Deletes an entity.
        /// </summary>
        /// <param name="entity">Entity to be deleted</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public Task DeleteAsync(
            TEntity           entity,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes an entity by primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public Task DeleteAsync(
            TPrimaryKey       id,
            CancellationToken cancellationToken = default);

        /// <summary>
        ///     Deletes many entities by function.
        ///     Notice that: All entities fits to given predicate are retrieved and deleted.
        ///     This may cause major performance problems if there are too many entities with
        ///     given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="cancellationToken">CancellationToken</param>
        public Task DeleteAsync(
            Expression<Func<TEntity, bool>> predicate,
            CancellationToken               cancellationToken = default);

        #endregion
    }
}