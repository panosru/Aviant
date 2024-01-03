using Aviant.Application.Identity;
using Aviant.Infrastructure.Identity.Persistence.Contexts;
using Aviant.Core.Entities;
using Aviant.Infrastructure.Persistence.Repository;

namespace Aviant.Infrastructure.Identity.Persistence.Repository;

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
