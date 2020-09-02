namespace Aviant.DDD.Domain.Persistence
{
    using System.Threading.Tasks;
    using Aggregates;

    public interface IEventsRepository<TAggregateRoot, in TAggregateId>
        where TAggregateRoot : class, IAggregateRoot<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        Task AppendAsync(TAggregateRoot aggregateRoot);

        Task<TAggregateRoot> RehydrateAsync(TAggregateId aggregateId);
    }
}