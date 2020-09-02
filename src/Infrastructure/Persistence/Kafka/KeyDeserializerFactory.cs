namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using Confluent.Kafka;
    using Domain.Aggregates;

    internal class KeyDeserializerFactory
    {
        public IDeserializer<TAggregateId> Create<TDeserializer, TAggregateId>()
            where TAggregateId : class, IAggregateId
            where TDeserializer : class, IDeserializer<TAggregateId>, new() => new TDeserializer();
    }
}