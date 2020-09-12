namespace Aviant.DDD.Application.Persistance
{
    #region

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    #endregion

    public interface IDbContextWrite : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}