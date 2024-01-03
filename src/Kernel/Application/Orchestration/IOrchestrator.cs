using Aviant.Application.Commands;
using Aviant.Application.Queries;

namespace Aviant.Application.Orchestration;

public interface IOrchestrator
{
    public Task<OrchestratorResponse> SendCommandAsync<T>(
        ICommand<T>       command,
        CancellationToken cancellationToken = default);

    public Task<OrchestratorResponse> SendQueryAsync<T>(
        IQuery<T>         query,
        CancellationToken cancellationToken = default);
}
