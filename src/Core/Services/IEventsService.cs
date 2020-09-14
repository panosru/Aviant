namespace Aviant.DDD.Core.Services
{
    #region

    using System.Threading.Tasks;
    using Aggregates;

    #endregion

    public interface IEventsService<TAggregate, in TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        Task PersistAsync(TAggregate aggregate);

        Task<TAggregate> RehydrateAsync(TAggregateId key);
    }
}