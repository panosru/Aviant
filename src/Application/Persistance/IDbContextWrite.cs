namespace Aviant.DDD.Application.Persistance
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDbContextWrite : IDisposable
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}