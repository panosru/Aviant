namespace Aviant.DDD.Application.Persistance
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDbContextWrite : IDisposable
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
