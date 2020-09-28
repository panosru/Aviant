namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using Confluent.Kafka;
    using Core.Aggregates;

    internal sealed class KeyDeserializerFactory
    {
        public IDeserializer<TAggregateId> Create<TDeserializer, TAggregateId>()
            where TAggregateId : class, IAggregateId
            where TDeserializer : class, IDeserializer<TAggregateId>, new() => new TDeserializer();
    }
}