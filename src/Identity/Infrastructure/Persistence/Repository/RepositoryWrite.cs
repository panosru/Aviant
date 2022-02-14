namespace Aviant.Infrastructure.Identity.Persistence.Repository;

using Application.Identity;
using Contexts;
using Core.Entities;
using Infrastructure.Persistence.Repository;

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
