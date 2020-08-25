namespace Aviant.DDD.Domain.Events
{
    using Aggregates;

    public interface IEvent<out TKey>
    {
        long AggregateVersion { get; }
        
        TKey AggregateId { get; }
    }
}