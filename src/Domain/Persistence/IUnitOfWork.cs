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
        ///     Commit changes to database persistence
        /// </summary>
        /// <returns>Integer representing affected rows</returns>
        Task<int> Commit();

        /// <summary>
        ///     Commit changes to event sourcing persistence
        /// </summary>
        /// <param name="aggregateRoot"></param>
        /// <typeparam name="TAggregateRoot"></typeparam>
        /// <typeparam name="TAggregateId"></typeparam>
        /// <returns></returns>
        Task<bool> Commit<TAggregateRoot, TAggregateId>(TAggregateRoot aggregateRoot)
            where TAggregateRoot : class, IAggregateRoot<TAggregateId>
            where TAggregateId : class, IAggregateId;
    }
}