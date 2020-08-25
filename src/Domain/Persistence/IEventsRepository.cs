namespace Aviant.DDD.Domain.Persistence
{
    using System.Threading.Tasks;
    using Aggregates;

    public interface IEventsRepository<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {
        Task AppendAsync(TAggregateRoot aggregateRoot);

        Task<TAggregateRoot> RehydrateAsync(TKey key);
    }
}