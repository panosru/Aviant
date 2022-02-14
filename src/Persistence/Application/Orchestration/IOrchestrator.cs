namespace Aviant.Application.Persistence.Orchestration;

using Application.Orchestration;
using Commands;
using Persistence;
using Queries;

// ReSharper disable once UnusedTypeParameter
public interface IOrchestrator<TDbContext>
    where TDbContext : IDbContextWrite
{
    public Task<OrchestratorResponse> SendCommandAsync<T>(
        ICommand<T>       command,
        CancellationToken cancellationToken = default);

    public Task<OrchestratorResponse> SendQueryAsync<T>(
        IQuery<T>         query,
        CancellationToken cancellationToken = default);
}
