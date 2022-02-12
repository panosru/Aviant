namespace Aviant.EventSourcing.Application.Orchestration;

using Aviant.Application.Orchestration;
using Aviant.Application.Queries;
using Commands;
using Core.Aggregates;

public interface IOrchestrator<in TAggregate, out TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    public Task<OrchestratorResponse> SendCommandAsync(
        ICommand<TAggregate, TAggregateId> command,
        CancellationToken                  cancellationToken = default);

    public Task<OrchestratorResponse> SendQueryAsync<T>(
        IQuery<T>         query,
        CancellationToken cancellationToken = default);
}
