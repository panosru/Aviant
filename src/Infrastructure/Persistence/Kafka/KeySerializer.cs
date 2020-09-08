namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using Confluent.Kafka;
    using Domain.Aggregates;

    internal class KeySerializer<TAggregateId> : ISerializer<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        #region ISerializer<TAggregateId> Members

        public byte[] Serialize(TAggregateId data, SerializationContext context) => data.Serialize();

        #endregion
    }
}