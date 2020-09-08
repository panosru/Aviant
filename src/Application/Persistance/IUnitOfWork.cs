namespace Aviant.DDD.Application.Persistance
{
    using System.Threading.Tasks;
    using Domain.Aggregates;

    /// <summary>
    ///     Unit of Work Interface
    /// </summary>
    public interface IUnitOfWork<TDbContext>
        where TDbContext : IApplicationDbContext
    {
        /// <summary>
        ///     Commit changes to database persistence
        /// </summary>
        /// <returns>Integer representing affected rows</returns>
        Task<int> Commit();
    }

    public interface IUnitOfWork<in TAggregateRoot, TAggregateId>
        where TAggregateRoot : class, IAggregateRoot<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        /// <summary>
        ///     Commit changes to event sourcing persistence
        /// </summary>
        /// <param name="aggregateRoot"></param>
        /// <returns></returns>
        Task<bool> Commit(TAggregateRoot aggregateRoot);
    }
}