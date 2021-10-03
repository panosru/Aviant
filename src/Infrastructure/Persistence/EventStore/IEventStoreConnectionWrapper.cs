namespace Aviant.DDD.Infrastructure.Persistence.EventStore
{
    using System.Threading;
    using System.Threading.Tasks;
    using global::EventStore.ClientAPI;

    public interface IEventStoreConnectionWrapper
    {
        public Task<IEventStoreConnection> GetConnectionAsync(CancellationToken cancellationToken = default);
    }
}
