namespace Aviant.DDD.Infrastructure.Persistence.Repository;

using System.Linq.Expressions;
using Application.Identity;
using Contexts;
using Core.Entities;
using Core.Exceptions;
using Core.Persistence;
using Microsoft.EntityFrameworkCore;

/// <inheritdoc cref="Aviant.DDD.Core.Persistence.IRepositoryWrite&lt;TEntity,TPrimaryKey&gt;" />
public abstract class RepositoryWriteBase<TDbContext, TEntity, TPrimaryKey>
    : IRepositoryWrite<TEntity, TPrimaryKey>,
      IRepositoryImplementation<TEntity, TPrimaryKey>
    where TDbContext : DbContext
    where TEntity : Entity<TPrimaryKey>
{
    private readonly IRepositoryImplementation<TEntity, TPrimaryKey> _repositoryImplementation;

    protected RepositoryWriteBase(TDbContext dbContext)
    {
        _repositoryImplementation = this;

        DbContext = dbContext;
    }

    private TDbContext DbContext { get; }

    private DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

    #region IRepositoryWrite<TEntity,TPrimaryKey> Members

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

    ~RepositoryWriteBase()
    {
        Dispose(false);
    }

    private TEntity Get(TPrimaryKey id) =>
        DbSet
           .FirstOrDefault(_repositoryImplementation.CreateEqualityExpressionForId(id))
     ?? throw new EntityNotFoundException(typeof(TEntity), id);

    private ValueTask<TEntity> GetAsync(TPrimaryKey id) =>
        new(Get(id));

    private List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate) =>
        DbSet.Where(predicate).ToList();

    private Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate) =>
        Task.FromResult(GetAllList(predicate));

    #region Insert

    public virtual TEntity Insert(TEntity entity)
    {
        // Run the task in async mode
        Task.Run(
                async () =>
                {
                    // Run entity validation method first
                    await entity.ValidateAsync()
                       .ConfigureAwait(false);
                })
           .GetAwaiter()
           .GetResult(); // to get the exception, if any

        DbSet.Add(entity);

        return entity;
    }

    public virtual Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        Task.FromResult(Insert(entity));

    public virtual TPrimaryKey InsertAndGetId(TEntity entity) =>
        Insert(entity).Id;

    public virtual async Task<TPrimaryKey> InsertAndGetIdAsync(
        TEntity           entity,
        CancellationToken cancellationToken = default)
    {
        var insertedEntity = await InsertAsync(entity, cancellationToken)
           .ConfigureAwait(false);

        return insertedEntity.Id;
    }

    public virtual TEntity InsertOrUpdate(TEntity entity) =>
        entity.IsTransient()
            ? Insert(entity)
            : Update(entity);

    public virtual async Task<TEntity> InsertOrUpdateAsync(
        TEntity           entity,
        CancellationToken cancellationToken = default) =>
        entity.IsTransient()
            ? await InsertAsync(entity, cancellationToken).ConfigureAwait(false)
            : await UpdateAsync(entity, cancellationToken).ConfigureAwait(false);

    public virtual TPrimaryKey InsertOrUpdateAndGetId(TEntity entity) =>
        InsertOrUpdate(entity).Id;

    public virtual async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(
        TEntity           entity,
        CancellationToken cancellationToken = default)
    {
        var insertedEntity = await InsertOrUpdateAsync(entity, cancellationToken)
           .ConfigureAwait(false);

        return insertedEntity.Id;
    }

    #endregion

    #region Update

    public virtual TEntity Update(TEntity entity)
    {
        // Run the task in async mode
        Task.Run(
                async () =>
                {
                    // Run entity validation method first
                    await entity.ValidateAsync()
                       .ConfigureAwait(false);
                })
           .GetAwaiter()
           .GetResult(); // to get the exception, if any

        DbContext.Entry(entity).State = EntityState.Modified;

        return entity;
    }

    public virtual TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
    {
        var entity = DbSet
                        .FirstOrDefault(_repositoryImplementation.CreateEqualityExpressionForId(id))
                  ?? throw new EntityNotFoundException(nameof(id));

        updateAction(entity);
        DbContext.Entry(entity).State = EntityState.Modified;
        return entity;
    }

    public virtual Task<TEntity> UpdateAsync(
        TEntity           entity,
        CancellationToken cancellationToken = default) =>
        Task.FromResult(Update(entity));

    public virtual async Task<TEntity> UpdateAsync(
        TPrimaryKey         id,
        Func<TEntity, Task> updateAction,
        CancellationToken   cancellationToken = default)
    {
        var entity = await GetAsync(id)
           .ConfigureAwait(false);

        await updateAction(entity).ConfigureAwait(false);

        DbContext.Entry(entity).State = EntityState.Modified;

        return entity;
    }

    #endregion

    #region Delete

    public virtual void Delete(TEntity entity)
    {
        // Run the task in async mode
        Task.Run(
                async () =>
                {
                    // Run entity validation method first
                    await entity.ValidateAsync()
                       .ConfigureAwait(false);
                })
           .GetAwaiter()
           .GetResult(); // to get the exception, if any

        DbContext.Entry(entity).State = EntityState.Deleted;
    }

    public virtual void Delete(TPrimaryKey id)
    {
        var entity = Get(id);
        Delete(entity);
    }

    public virtual void Delete(Expression<Func<TEntity, bool>> predicate)
    {
        foreach (var entity in GetAllList(predicate))
            Delete(entity);
    }

    public virtual Task DeleteAsync(
        TEntity           entity,
        CancellationToken cancellationToken = default)
    {
        Delete(entity);

        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(
        TPrimaryKey       id,
        CancellationToken cancellationToken = default)
    {
        Delete(id);

        return Task.CompletedTask;
    }

    public virtual async Task DeleteAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken               cancellationToken = default)
    {
        List<TEntity> entities = await GetAllListAsync(predicate)
           .ConfigureAwait(false);

        foreach (var entity in entities)
            await DeleteAsync(entity, cancellationToken)
               .ConfigureAwait(false);
    }

    #endregion
}

public abstract class RepositoryWrite<TDbContext, TEntity, TPrimaryKey>
    : RepositoryWriteBase<TDbContext, TEntity, TPrimaryKey>
    where TEntity : Entity<TPrimaryKey>
    where TDbContext : DbContextWrite<TDbContext>
{
    protected RepositoryWrite(TDbContext context)
        : base(context)
    { }
}

public abstract class RepositoryWrite<TDbContext, TApplicationUser, TApplicationRole, TEntity, TPrimaryKey>
    : RepositoryWriteBase<TDbContext, TEntity, TPrimaryKey>
    where TEntity : Entity<TPrimaryKey>
    where TApplicationUser : ApplicationUser
    where TApplicationRole : ApplicationRole
    where TDbContext : AuthorizationDbContextWrite<TDbContext, TApplicationUser, TApplicationRole>
{
    protected RepositoryWrite(TDbContext context)
        : base(context)
    { }
}
