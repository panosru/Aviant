using Aviant.Application.Identity;
using Aviant.Infrastructure.Identity.Persistence.Contexts;
using Aviant.Core.Entities;
using Aviant.Infrastructure.Persistence.Repository;

namespace Aviant.Infrastructure.Identity.Persistence.Repository;

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
