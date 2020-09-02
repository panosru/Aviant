namespace Aviant.DDD.Domain.Events
{
    using Aggregates;

    public interface IEvent<out TAggregateId>
        where TAggregateId : IAggregateId
    {
        long AggregateVersion { get; }

        TAggregateId AggregateId { get; }
    }
}