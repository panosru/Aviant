namespace Aviant.DDD.Core.Services
{
    using Aggregates;
    using DomainEvents;

    public interface IEventDeserializer
    {
        public IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string type, byte[] data)
            where TAggregateId : IAggregateId;

        public IDomainEvent<TAggregateId> Deserialize<TAggregateId>(string type, string data)
            where TAggregateId : IAggregateId;
    }
}
