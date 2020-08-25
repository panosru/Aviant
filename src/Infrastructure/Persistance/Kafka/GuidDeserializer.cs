namespace Aviant.DDD.Infrastructure.Persistance.Kafka
{
    using System;
    using Confluent.Kafka;

    internal class GuidDeserializer : IDeserializer<Guid>
    {
        public Guid Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return new Guid(data);
        }
    }
}