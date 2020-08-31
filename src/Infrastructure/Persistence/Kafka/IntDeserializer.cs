namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using System;
    using Confluent.Kafka;

    public class IntDeserializer : IDeserializer<int>
    {
        public int Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return BitConverter.ToInt32(data);
        }
    }
}