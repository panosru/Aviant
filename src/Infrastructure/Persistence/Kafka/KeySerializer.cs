namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using Confluent.Kafka;
    using Core.Aggregates;

    internal class KeySerializer<TAggregateId> : ISerializer<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        #region ISerializer<TAggregateId> Members

        public byte[] Serialize(TAggregateId aggregateId, SerializationContext context) => aggregateId.Serialize();

        #endregion
    }
}