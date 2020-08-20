namespace Aviant.DDD.Application.Orchestration
{
    using System.Threading.Tasks;
    using Commands;
    using Queries;

    public interface IOrchestrator
    {
        Task<RequestResult> SendCommand<T>(ICommand<T> command);

        Task<RequestResult> SendQuery<T>(IQuery<T> command);
    }
}