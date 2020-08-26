namespace Aviant.DDD.Domain.Events
{
    public interface IEvent<out TKey>
    {
        long AggregateVersion { get; }

        TKey AggregateId { get; }
    }
}