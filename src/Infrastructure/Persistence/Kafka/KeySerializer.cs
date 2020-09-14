namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    #region

    using Confluent.Kafka;
    using Core.Aggregates;

    #endregion

    internal class KeySerializer<TAggregateId> : ISerializer<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        #region ISerializer<TAggregateId> Members

        public byte[] Serialize(TAggregateId aggregateId, SerializationContext context) => aggregateId.Serialize();

        #endregion
    }
}