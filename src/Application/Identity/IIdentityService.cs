namespace Aviant.DDD.Application.Identity
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IIdentityService
    {
        public Task<object?> AuthenticateAsync(
            string            username,
            string            password,
            CancellationToken cancellationToken = default);

        public Task<IdentityResult> ConfirmEmailAsync(
            string            toekn,
            string            email,
            CancellationToken cancellationToken = default);

        public Task<string> GetUserNameAsync(
            Guid              userId,
            CancellationToken cancellationToken = default);

        public Task<(IdentityResult Result, Guid UserId)> CreateUserAsync(
            string            username,
            string            password,
            CancellationToken cancellationToken = default);

        public Task<IdentityResult> DeleteUserAsync(
            Guid              userId,
            CancellationToken cancellationToken = default);
    }
}