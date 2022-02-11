namespace Aviant.Infrastructure.Transport.Kafka;

using Confluent.Kafka;
using Core.Aggregates;

internal sealed class KeySerializer<TAggregateId> : ISerializer<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    #region ISerializer<TAggregateId> Members

    public byte[] Serialize(TAggregateId aggregateId, SerializationContext context) => aggregateId.Serialize();

    #endregion
}
