namespace Aviant.DDD.Infrastructure.Persistence.Kafka
{
    using System;
    using Confluent.Kafka;

    internal class KeyDeserializerFactory
    {
        public IDeserializer<TKey> Create<TKey>()
        {
            var tk = typeof(TKey);
            if (typeof(Guid) == tk)
                return (dynamic) new GuidDeserializer();

            throw new ArgumentOutOfRangeException($"Invalid type: {tk}");
        }
    }
}