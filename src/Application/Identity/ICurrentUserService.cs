namespace Aviant.DDD.Application.Identity
{
    #region

    using System;

    #endregion

    public interface ICurrentUserService
    {
        Guid UserId { get; }
    }
}