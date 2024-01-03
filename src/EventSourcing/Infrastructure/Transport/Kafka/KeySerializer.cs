using Confluent.Kafka;
using Aviant.Core.EventSourcing.Aggregates;

namespace Aviant.Infrastructure.EventSourcing.Transport.Kafka;

internal sealed class KeySerializer<TAggregateId> : ISerializer<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    #region ISerializer<TAggregateId> Members

    public byte[] Serialize(TAggregateId aggregateId, SerializationContext context) => aggregateId.Serialize();

    #endregion
}
