using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Aviant.Infrastructure.Identity.Persistence.Contexts;

/// <inheritdoc cref="Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext{TUser, TRole, TKey}" />
/// <summary>
///     Database abstraction for a combined <see cref="T:Microsoft.EntityFrameworkCore.DbContext" /> using ASP.NET Identity
///     and Identity Server.
/// </summary>
/// <typeparam name="TUser"></typeparam>
/// <typeparam name="TRole"></typeparam>
/// <typeparam name="TKey">Key of the IdentityUser entity</typeparam>
public class AuthorizationDbContext<TUser, TRole, TKey>
    : IdentityDbContext<TUser, TRole, TKey>
    where TUser : IdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TKey : IEquatable<TKey>
{
    /// <inheritdoc />
    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="T:Aviant.Infrastructure.Persistence.Contexts.ApiAuthorizationDbContext`1" />.
    /// </summary>
    public AuthorizationDbContext(DbContextOptions options) : base(options)
    { }

    /// <inheritdoc />
    protected AuthorizationDbContext()
    { }
}

/// <inheritdoc />
/// <summary>
///     Database abstraction for a combined <see cref="T:Microsoft.EntityFrameworkCore.DbContext" /> using ASP.NET Identity
///     and Identity Server.
/// </summary>
/// <typeparam name="TUser"></typeparam>
public class AuthorizationDbContext<TUser> : AuthorizationDbContext<TUser, IdentityRole, string>
    where TUser : IdentityUser
{
/// <inheritdoc />
    /// <summary>
    ///     Initializes a new instance of
    ///     <see cref="T:Aviant.Infrastructure.Persistence.Contexts.ApiAuthorizationDbContext`1" />.
    /// </summary>
    public AuthorizationDbContext(DbContextOptions options) : base(options)
    { }

    /// <inheritdoc />
    protected AuthorizationDbContext()
    { }
}
