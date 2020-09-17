namespace Aviant.DDD.Core.Aggregates
{
    using System;
    using System.Text;
    using System.Text.Json;

    public abstract class AggregateId<TKey> : IAggregateId<TKey>
        where TKey : notnull
    {
        protected AggregateId(TKey key) => Key = key;

        #region IAggregateId<TKey> Members

        public TKey Key { get; }

        public virtual byte[] Serialize()
        {
            return Key switch
            {
                Guid guid      => Encoding.UTF8.GetBytes(guid.ToString()), //guid.ToByteArray(),
                int @int       => Encoding.UTF8.GetBytes(@int.ToString()), //BitConverter.GetBytes(@int),
                string @string => Encoding.UTF8.GetBytes(@string),
                _              => Encoding.UTF8.GetBytes(JsonSerializer.Serialize(Key))
            };
        }

        #endregion

        public override string ToString()
        {
            return Key switch
            {
                Guid guid => guid.ToString(),
                string id => id,
                int id    => id.ToString(),
                _         => (string) (Key as object)
            };
        }
    }
}