namespace Aviant.DDD.Domain.Services
{
    #region

    using Aggregates;
    using Events;

    #endregion

    public interface IEventDeserializer
    {
        IEvent<TAggregateId> Deserialize<TAggregateId>(string type, byte[] data)
            where TAggregateId : IAggregateId;

        IEvent<TAggregateId> Deserialize<TAggregateId>(string type, string data)
            where TAggregateId : IAggregateId;
    }
}