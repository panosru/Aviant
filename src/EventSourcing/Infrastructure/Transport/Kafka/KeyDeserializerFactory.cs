namespace Aviant.Infrastructure.EventSourcing.Transport.Kafka;

using Confluent.Kafka;
using Core.EventSourcing.Aggregates;

internal sealed class KeyDeserializerFactory
{
    public static IDeserializer<TAggregateId> Create<TDeserializer, TAggregateId>()
        where TAggregateId : class, IAggregateId
        where TDeserializer : class, IDeserializer<TAggregateId>, new() => new TDeserializer();
}
