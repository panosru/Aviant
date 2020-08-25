namespace Aviant.DDD.Domain.Aggregates
{
    public interface IAggregateId<out T>
        where T : notnull
    {
        T Id { get; }
    }
}