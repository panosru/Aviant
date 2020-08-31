namespace Aviant.DDD.Domain.Persistence
{
    using System.Threading.Tasks;
    using Aggregates;

    /// <summary>
    ///     Unit of Work Interface
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        ///     Commit changes to database
        /// </summary>
        /// <returns>Integer representing affected rows</returns>
        Task<int> Commit();

        Task<bool> Commit<TAggregateRoot, TKey>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, IAggregateRoot<TKey>;
    }
}