namespace Aviant.DDD.Core.Services
{
    using Aggregates;
    using Events;

    public interface IEventDeserializer
    {
        IEvent<TAggregateId> Deserialize<TAggregateId>(string type, byte[] data)
            where TAggregateId : IAggregateId;

        IEvent<TAggregateId> Deserialize<TAggregateId>(string type, string data)
            where TAggregateId : IAggregateId;
    }
}