namespace Aviant.DDD.Domain.Aggregates
{
    using System;

    public class AggregateId<T> : IAggregateId<T>
        where T : notnull
    {
        public AggregateId(T id)
        {
            Id = id;
        }

        public T Id { get; }

        public override string ToString()
        {
            return Id switch
            {
                Guid guid => guid.ToString(),
                string id => id,
                int id => id.ToString(),
                _ => (string) (Id as object)
            };
        }
    }
}