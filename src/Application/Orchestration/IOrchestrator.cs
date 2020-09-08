namespace Aviant.DDD.Application.Orchestration
{
    using System.Threading.Tasks;
    using Commands;
    using Domain.Aggregates;
    using Persistance;
    using Queries;

    public interface IOrchestrator
    {
        Task<RequestResult> SendCommand<T>(ICommand<T> command);

        Task<RequestResult> SendQuery<T>(IQuery<T> query);
    }

    public interface IOrchestrator<TDbContext>
        where TDbContext : IApplicationDbContext
    {
        Task<RequestResult> SendCommand<T>(ICommand<T> command);

        Task<RequestResult> SendQuery<T>(IQuery<T> query);
    }

    public interface IOrchestrator<in TAggregateRoot, out TAggregateId>
        where TAggregateRoot : class, IAggregateRoot<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        Task<RequestResult> SendCommand(ICommand<TAggregateRoot, TAggregateId> command);

        Task<RequestResult> SendQuery<T>(IQuery<T> query);
    }
}