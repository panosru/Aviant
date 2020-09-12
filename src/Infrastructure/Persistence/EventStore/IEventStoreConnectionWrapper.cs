namespace Aviant.DDD.Infrastructure.Persistence.EventStore
{
    #region

    using System.Threading.Tasks;
    using global::EventStore.ClientAPI;

    #endregion

    public interface IEventStoreConnectionWrapper
    {
        Task<IEventStoreConnection> GetConnectionAsync();
    }
}