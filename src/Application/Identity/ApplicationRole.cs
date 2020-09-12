namespace Aviant.DDD.Application.Identity
{
    #region

    using System;
    using Microsoft.AspNetCore.Identity;

    #endregion

    public abstract class ApplicationRole : IdentityRole<Guid>
    { }
}