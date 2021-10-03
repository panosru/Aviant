namespace Aviant.DDD.Core.DomainEvents
{
    using Aggregates;

    /// <summary>
    ///     Domain Event Interface
    /// </summary>
    /// <typeparam name="TAggregateId">The expected type of Aggregate Id</typeparam>
    public interface IDomainEvent<out TAggregateId>
        where TAggregateId : IAggregateId
    {
        /// <summary>
        ///     The current aggregate version
        /// </summary>
        public long AggregateVersion { get; }

        /// <summary>
        ///     The current aggregate id
        /// </summary>
        public TAggregateId AggregateId { get; }
    }
}
