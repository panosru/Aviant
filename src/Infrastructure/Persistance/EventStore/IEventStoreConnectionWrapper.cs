namespace Aviant.DDD.Infrastructure.Persistance.EventStore
{
    using System.Threading.Tasks;
    using global::EventStore.ClientAPI;

    public interface IEventStoreConnectionWrapper
    {
        Task<IEventStoreConnection> GetConnectionAsync();
    }
}