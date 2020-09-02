namespace Aviant.DDD.Domain.Aggregates
{
    public interface IAggregateId
    {
        byte[] Serialize();
    }

    public interface IAggregateId<out T> : IAggregateId
        where T : notnull
    {
        T Key { get; }
    }
}