using EventStore.ClientAPI;

namespace Aviant.Infrastructure.EventSourcing.Persistence.EventStore;

public interface IEventStoreConnectionWrapper
{
    public Task<IEventStoreConnection> GetConnectionAsync(CancellationToken cancellationToken = default);
}
