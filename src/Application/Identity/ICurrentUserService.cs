namespace Aviant.DDD.Application.Identity
{
    using System;

    public interface ICurrentUserService
    {
        public Guid UserId { get; }
    }
}
