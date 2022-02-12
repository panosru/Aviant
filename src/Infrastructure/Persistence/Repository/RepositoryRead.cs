namespace Aviant.Foundation.Infrastructure.Persistence.Repository;

using System.Linq.Expressions;
using Application.Identity;
using Contexts;
using Core.Entities;
using Core.Exceptions;
using Core.Persistence;
using Microsoft.EntityFrameworkCore;

/// <inheritdoc cref="Aviant.Foundation.Core.Persistence.IRepositoryRead{TEntity,TPrimaryKey}" />
public abstract class RepositoryReadBase<TDbContext, TEntity, TPrimaryKey>
    : IRepositoryRead<TEntity, TPrimaryKey>,
      IRepositoryImplementation<TEntity, TPrimaryKey>
    where TDbContext : DbContext
    where TEntity : Entity<TPrimaryKey>
{
    private readonly IRepositoryImplementation<TEntity, TPrimaryKey> _repositoryImplementation;

    protected RepositoryReadBase(TDbContext dbContext)
    {
        _repositoryImplementation = this;

        DbContext = dbContext;
    }

    private TDbContext DbContext { get; }

    private DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

    #region IRepositoryRead<TEntity,TPrimaryKey> Members

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

    ~RepositoryReadBase()
    {
        Dispose(false);
    }

    #region Select/Get/Query

    public virtual IQueryable<TEntity> GetAll() => DbSet;

    public virtual IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
    {
        IQueryable<TEntity> query = GetAll();

        return propertySelectors.Aggregate(
            query,
            (current, includeProperty) =>
                current.Include(includeProperty));
    }

    public virtual IQueryable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate) =>
        GetAll().Where(predicate);

    public virtual IQueryable<TEntity> FindByIncluding(
        Expression<Func<TEntity, bool>>            predicate,
        params Expression<Func<TEntity, object>>[] propertySelectors) =>
        GetAllIncluding(propertySelectors).Where(predicate);

    public virtual List<TEntity> GetAllList() =>
        GetAll().ToList();

    public virtual List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate) =>
        FindBy(predicate).ToList();

    public virtual ValueTask<List<TEntity>> GetAllListAsync(CancellationToken cancellationToken = default) =>
        new(GetAllList());

    public virtual ValueTask<List<TEntity>> GetAllListAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken               cancellationToken = default) =>
        new(GetAllList(predicate));


    public virtual TEntity GetAllListIncluding(
        Expression<Func<TEntity, bool>>            predicate,
        params Expression<Func<TEntity, object>>[] propertySelectors) =>
        FindByIncluding(predicate, propertySelectors)
           .FirstOrDefault(predicate)
     ?? throw new EntityNotFoundException(nameof(predicate));

    public virtual ValueTask<TEntity> GetAllListIncludingAsync(
        Expression<Func<TEntity, bool>>            predicate,
        CancellationToken                          cancellationToken = default,
        params Expression<Func<TEntity, object>>[] propertySelectors) =>
        new(GetAllListIncluding(predicate, propertySelectors));

    public virtual TEntity Get(TPrimaryKey id) =>
        FirstOrDefault(id)
     ?? throw new EntityNotFoundException(typeof(TEntity), id);

    public virtual ValueTask<TEntity> GetAsync(
        TPrimaryKey       id,
        CancellationToken cancellationToken = default) =>
        new(Get(id));


    public virtual TEntity Single(Expression<Func<TEntity, bool>> predicate) =>
        GetAll().Single(predicate);

    public virtual ValueTask<TEntity> GetSingleAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken               cancellationToken = default) =>
        new(Single(predicate));

    public virtual TEntity GetSingleIncluding(
        Expression<Func<TEntity, bool>>            predicate,
        params Expression<Func<TEntity, object>>[] propertySelectors) =>
        GetAllIncluding(propertySelectors)
           .SingleOrDefault(predicate)
     ?? throw new EntityNotFoundException(nameof(predicate));

    public virtual ValueTask<TEntity> GetSingleIncludingAsync(
        Expression<Func<TEntity, bool>>            predicate,
        CancellationToken                          cancellationToken = default,
        params Expression<Func<TEntity, object>>[] propertySelectors) =>
        new(GetSingleIncluding(predicate, propertySelectors));

    public virtual TEntity FirstOrDefault(TPrimaryKey id) =>
        GetAll().FirstOrDefault(_repositoryImplementation.CreateEqualityExpressionForId(id))
     ?? throw new EntityNotFoundException(nameof(id));

    public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate) =>
        GetAll().FirstOrDefault(predicate)
     ?? throw new EntityNotFoundException(nameof(predicate));

    public virtual ValueTask<TEntity> FirstOrDefaultAsync(
        TPrimaryKey       id,
        CancellationToken cancellationToken = default) =>
        new(FirstOrDefault(id));

    public virtual ValueTask<TEntity> FirstOrDefaultAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken               cancellationToken = default) =>
        new(FirstOrDefault(predicate));

    public virtual TEntity FirstOrDefaultIncluding(
        TPrimaryKey                                id,
        params Expression<Func<TEntity, object>>[] propertySelectors) =>
        GetAllIncluding(propertySelectors)
           .FirstOrDefault(_repositoryImplementation.CreateEqualityExpressionForId(id))
     ?? throw new EntityNotFoundException(nameof(id));

    public virtual TEntity FirstOrDefaultIncluding(
        Expression<Func<TEntity, bool>>            predicate,
        params Expression<Func<TEntity, object>>[] propertySelectors) =>
        GetAllIncluding(propertySelectors)
           .FirstOrDefault(predicate)
     ?? throw new EntityNotFoundException(nameof(predicate));

    public virtual ValueTask<TEntity> FirstOrDefaultIncludingAsync(
        TPrimaryKey                                id,
        CancellationToken                          cancellationToken = default,
        params Expression<Func<TEntity, object>>[] propertySelectors) =>
        new(FirstOrDefaultIncluding(id, propertySelectors));

    public virtual ValueTask<TEntity> FirstOrDefaultIncludingAsync(
        Expression<Func<TEntity, bool>>            predicate,
        CancellationToken                          cancellationToken = default,
        params Expression<Func<TEntity, object>>[] propertySelectors) =>
        new(FirstOrDefaultIncluding(predicate, propertySelectors));

    #endregion

    #region Aggregates

    public virtual ValueTask<bool> AnyAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken               cancellationToken = default) =>
        new(DbSet.AnyAsync(predicate, cancellationToken));

    public virtual ValueTask<bool> AllAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken               cancellationToken = default) =>
        new(DbSet.AllAsync(predicate, cancellationToken));

    public virtual int Count() =>
        GetAll().Count();

    public virtual int Count(Expression<Func<TEntity, bool>> predicate) =>
        GetAll().Count(predicate);

    public virtual ValueTask<int> CountAsync(CancellationToken cancellationToken = default) =>
        new(Count());

    public virtual ValueTask<int> CountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken               cancellationToken = default) =>
        new(Count(predicate));

    public virtual long LongCount() =>
        GetAll().LongCount();

    public virtual long LongCount(Expression<Func<TEntity, bool>> predicate) =>
        GetAll().LongCount(predicate);

    public virtual ValueTask<long> LongCountAsync(CancellationToken cancellationToken = default) =>
        new(LongCount());

    public virtual ValueTask<long> LongCountAsync(
        Expression<Func<TEntity, bool>> predicate,
        CancellationToken               cancellationToken = default) =>
        new(LongCount(predicate));

    #endregion
}

public abstract class RepositoryRead<TDbContext, TEntity, TPrimaryKey>
    : RepositoryReadBase<TDbContext, TEntity, TPrimaryKey>
    where TEntity : Entity<TPrimaryKey>
    where TDbContext : DbContextRead
{
    protected RepositoryRead(TDbContext context)
        : base(context)
    { }
}

public abstract class RepositoryRead<TDbContext, TApplicationUser, TApplicationRole, TEntity, TPrimaryKey>
    : RepositoryReadBase<TDbContext, TEntity, TPrimaryKey>
    where TEntity : Entity<TPrimaryKey>
    where TApplicationUser : ApplicationUser
    where TApplicationRole : ApplicationRole
    where TDbContext : AuthorizationDbContextRead<TApplicationUser, TApplicationRole>
{
    protected RepositoryRead(TDbContext context)
        : base(context)
    { }
}
