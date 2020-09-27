namespace Aviant.DDD.Core.Services
{
    using System.Threading;
    using System.Threading.Tasks;
    using Aggregates;

    public interface IEventsService<TAggregate, in TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        public Task PersistAsync(
            TAggregate        aggregate,
            CancellationToken cancellationToken = default);

        public Task<TAggregate> RehydrateAsync(
            TAggregateId      key,
            CancellationToken cancellationToken = default);
    }
}