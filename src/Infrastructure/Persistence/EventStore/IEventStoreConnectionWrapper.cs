namespace Aviant.DDD.Infrastructure.Persistence.EventStore;

using global::EventStore.ClientAPI;

public interface IEventStoreConnectionWrapper
{
    public Task<IEventStoreConnection> GetConnectionAsync(CancellationToken cancellationToken = default);
}
