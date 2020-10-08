namespace Aviant.DDD.Core.DomainEvents
{
    using Aggregates;

    public interface IDomainEvent<out TAggregateId>
        where TAggregateId : IAggregateId
    {
        public long AggregateVersion { get; }

        public TAggregateId AggregateId { get; }
    }
}