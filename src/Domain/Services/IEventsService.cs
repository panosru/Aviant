namespace Aviant.DDD.Domain.Services
{
    using System.Threading.Tasks;
    using Aggregates;

    public interface IEventsService<TAggregate, in TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        Task PersistAsync(TAggregate aggregate);

        Task<TAggregate> RehydrateAsync(TAggregateId key);
    }
}