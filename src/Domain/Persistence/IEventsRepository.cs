namespace Aviant.DDD.Domain.Persistence
{
    #region

    using System.Threading.Tasks;
    using Aggregates;

    #endregion

    public interface IEventsRepository<TAggregate, in TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        Task AppendAsync(TAggregate aggregate);

        Task<TAggregate> RehydrateAsync(TAggregateId aggregateId);
    }
}