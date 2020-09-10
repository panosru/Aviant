namespace Aviant.DDD.Infrastructure.Persistence.Repository
{
    using Application.Identity;
    using Contexts;
    using Domain.Entities;

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