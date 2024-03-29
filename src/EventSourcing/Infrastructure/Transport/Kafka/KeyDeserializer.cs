using Confluent.Kafka;

namespace Aviant.Infrastructure.EventSourcing.Transport.Kafka;

internal sealed class KeyDeserializer : IDeserializer<Guid>
{
    #region IDeserializer<Guid> Members

    public Guid Deserialize(
        ReadOnlySpan<byte>   data,
        bool                 isNull,
        SerializationContext context) => new(data);

    #endregion
}
