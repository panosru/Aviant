namespace Aviant.DDD.Application.Identity
{
    #region

    using System;
    using Microsoft.AspNetCore.Identity;

    #endregion

    public abstract class ApplicationUser : IdentityUser<Guid>
    { }
}