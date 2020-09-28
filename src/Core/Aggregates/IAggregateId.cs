namespace Aviant.DDD.Core.Aggregates
{
    public interface IAggregateId
    {
        public byte[] Serialize();
    }

    internal interface IAggregateId<out T> : IAggregateId
        where T : notnull
    {
        public T Key { get; }
    }
}