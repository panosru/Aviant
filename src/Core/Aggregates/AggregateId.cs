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
            return Encoding.UTF8.GetBytes(ToString());
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