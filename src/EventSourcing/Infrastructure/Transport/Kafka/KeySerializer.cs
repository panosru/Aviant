namespace Aviant.Infrastructure.EventSourcing.Transport.Kafka;

using Confluent.Kafka;
using Core.EventSourcing.Aggregates;

internal sealed class KeySerializer<TAggregateId> : ISerializer<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    #region ISerializer<TAggregateId> Members

    public byte[] Serialize(TAggregateId aggregateId, SerializationContext context) => aggregateId.Serialize();

    #endregion
}
