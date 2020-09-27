namespace Aviant.DDD.Core.Aggregates
{
    public interface IAggregateId
    {
        public byte[] Serialize();
    }

    public interface IAggregateId<out T> : IAggregateId
        where T : notnull
    {
        public T Key { get; }
    }
}