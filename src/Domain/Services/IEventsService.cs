namespace Aviant.DDD.Domain.Services
{
    using System.Threading.Tasks;
    using Aggregates;

    public interface IEventsService<TAggregateRoot, TKey>
        where TAggregateRoot : class, IAggregateRoot<TKey>
    {
        Task PersistAsync(TAggregateRoot aggregateRoot);

        Task<TAggregateRoot> RehydrateAsync(TKey key);
    }
}