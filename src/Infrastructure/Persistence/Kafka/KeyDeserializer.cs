namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using System;
    using Confluent.Kafka;

    internal class KeyDeserializer : IDeserializer<Guid>
    {
        #region IDeserializer<Guid> Members

        public Guid Deserialize(
            ReadOnlySpan<byte>   data,
            bool                 isNull,
            SerializationContext context) => new Guid(data);

        #endregion
    }
}