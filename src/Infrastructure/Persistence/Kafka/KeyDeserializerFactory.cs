namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    #region

    using Confluent.Kafka;
    using Domain.Aggregates;

    #endregion

    internal class KeyDeserializerFactory
    {
        public IDeserializer<TAggregateId> Create<TDeserializer, TAggregateId>()
            where TAggregateId : class, IAggregateId
            where TDeserializer : class, IDeserializer<TAggregateId>, new() => new TDeserializer();
    }
}