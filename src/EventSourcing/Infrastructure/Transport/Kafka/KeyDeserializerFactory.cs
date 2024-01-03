using Confluent.Kafka;
using Aviant.Core.EventSourcing.Aggregates;

namespace Aviant.Infrastructure.EventSourcing.Transport.Kafka;

internal sealed class KeyDeserializerFactory
{
    public static IDeserializer<TAggregateId> Create<TDeserializer, TAggregateId>()
        where TAggregateId : class, IAggregateId
        where TDeserializer : class, IDeserializer<TAggregateId>, new() => new TDeserializer();
}
