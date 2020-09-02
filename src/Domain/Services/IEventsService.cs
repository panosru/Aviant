namespace Aviant.DDD.Domain.Services
{
    using System.Threading.Tasks;
    using Aggregates;

    public interface IEventsService<TAggregateRoot, in TAggregateId>
        where TAggregateRoot : class, IAggregateRoot<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        Task PersistAsync(TAggregateRoot aggregateRoot);

        Task<TAggregateRoot> RehydrateAsync(TAggregateId key);
    }
}