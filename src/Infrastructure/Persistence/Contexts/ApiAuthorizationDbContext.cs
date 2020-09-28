namespace Aviant.DDD.Infrastructure.Persistence.Contexts
{
    using System;
    using System.Threading.Tasks;
    using IdentityServer4.EntityFramework.Entities;
    using IdentityServer4.EntityFramework.Extensions;
    using IdentityServer4.EntityFramework.Interfaces;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    /// <inheritdoc cref="Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext&lt;TUser, TRole, TKey&gt;" />
    /// <summary>
    ///     Database abstraction for a combined <see cref="T:Microsoft.EntityFrameworkCore.DbContext" /> using ASP.NET Identity and Identity Server.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TKey">Key of the IdentityUser entity</typeparam>
    public class ApiAuthorizationDbContext<TUser, TRole, TKey>
        : IdentityDbContext<TUser, TRole, TKey>, IPersistedGrantDbContext
        where TUser : IdentityUser<TKey>
        where TRole : IdentityRole<TKey>
        where TKey : IEquatable<TKey>
    {
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of <see cref="T:Aviant.DDD.Infrastructure.Persistence.Contexts.ApiAuthorizationDbContext`3" />.
        /// </summary>
        /// <param name="options">The <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" />.</param>
        /// <param name="operationalStoreOptions">The <see cref="T:Microsoft.Extensions.Options.IOptions`1" />.</param>
        public ApiAuthorizationDbContext(
            DbContextOptions                  options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options) => _operationalStoreOptions = operationalStoreOptions;

        #region IPersistedGrantDbContext Members

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" />.
        /// </summary>
        public DbSet<PersistedGrant>? PersistedGrants { get; set; }

        /// <inheritdoc />
        /// <summary>
        ///     Gets or sets the <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" />.
        /// </summary>
        public DbSet<DeviceFlowCodes>? DeviceFlowCodes { get; set; }

        Task<int> IPersistedGrantDbContext.SaveChangesAsync() => base.SaveChangesAsync();

        #endregion

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///     Database abstraction for a combined <see cref="T:Microsoft.EntityFrameworkCore.DbContext" /> using ASP.NET Identity and Identity Server.
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class ApiAuthorizationDbContext<TUser> : ApiAuthorizationDbContext<TUser, IdentityRole, string>
        where TUser : IdentityUser
    {
        /// <inheritdoc />
        /// <summary>
        ///     Initializes a new instance of <see cref="T:Aviant.DDD.Infrastructure.Persistence.Contexts.ApiAuthorizationDbContext`1" />.
        /// </summary>
        /// <param name="options">The <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" />.</param>
        /// <param name="operationalStoreOptions">The <see cref="T:Microsoft.Extensions.Options.IOptions`1" />.</param>
        public ApiAuthorizationDbContext(
            DbContextOptions                  options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options, operationalStoreOptions)
        { }
    }
}