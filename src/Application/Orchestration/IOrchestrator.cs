namespace Aviant.DDD.Application.Orchestration
{
    using System.Threading.Tasks;
    using Commands;
    using Domain.Aggregates;
    using Queries;

    public interface IOrchestrator
    {
        Task<RequestResult> SendCommand<TAggregateRoot, TAggregateId>(ICommand<TAggregateRoot, TAggregateId> command)
            where TAggregateRoot : class, IAggregateRoot<TAggregateId>
            where TAggregateId : class, IAggregateId;

        Task<RequestResult> SendCommand<T>(ICommand<T> command);

        Task<RequestResult> SendQuery<T>(IQuery<T> query);
    }
}