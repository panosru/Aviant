namespace Aviant.DDD.Infrastructure.Persistence.Repository
{
    using Application.Identity;
    using Contexts;
    using Domain.Entities;

    public abstract class RepositoryWrite<TDbContext, TEntity, TPrimaryKey>
        : RepositoryWriteImplementation<TDbContext, TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TDbContext : DbContextWrite<TDbContext>
    {
        protected RepositoryWrite(TDbContext context)
            : base(context)
        { }
    }

    public abstract class RepositoryWrite<TDbContext, TApplicationUser, TApplicationRole, TEntity, TPrimaryKey>
        : RepositoryWriteImplementation<TDbContext, TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TApplicationUser : ApplicationUser
        where TApplicationRole : ApplicationRole
        where TDbContext : AuthorizationDbContextWrite<TDbContext, TApplicationUser, TApplicationRole>
    {
        protected RepositoryWrite(TDbContext context)
            : base(context)
        { }
    }
}