namespace Aviant.Infrastructure.Identity.Persistence.Repository;

using Application.Identity;
using Contexts;
using Core.Entities;
using Infrastructure.Persistence.Repository;

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
