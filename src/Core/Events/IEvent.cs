namespace Aviant.DDD.Core.Events
{
    using Aggregates;

    public interface IEvent<out TAggregateId>
        where TAggregateId : IAggregateId
    {
        public long AggregateVersion { get; }

        public TAggregateId AggregateId { get; }
    }
}