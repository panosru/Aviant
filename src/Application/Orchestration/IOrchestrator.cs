namespace Aviant.DDD.Application.Orchestration
{
    using System.Threading.Tasks;
    using Commands;
    using Domain.Aggregates;
    using Queries;

    public interface IOrchestrator
    {
        Task<RequestResult> SendCommand<TAggregateRoot, TKey>(ICommand<TAggregateRoot, TKey> command)
            where TAggregateRoot : class, IAggregateRoot<TKey>;
        
        Task<RequestResult> SendCommand<T>(ICommand<T> command);

        Task<RequestResult> SendQuery<T>(IQuery<T> query);
    }
}