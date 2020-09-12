namespace Aviant.DDD.Application.Identity
{
    #region

    using System;
    using System.Threading.Tasks;

    #endregion

    public interface IIdentityService
    {
        Task<object?> Authenticate(string username, string password);

        Task<IdentityResult> ConfirmEmail(string toekn, string email);

        Task<string> GetUserNameAsync(Guid userId);

        Task<(IdentityResult Result, Guid UserId)> CreateUserAsync(string username, string password);

        Task<IdentityResult> DeleteUserAsync(Guid userId);
    }
}