namespace Aviant.Application.EventSourcing.Orchestration;

using Aviant.Application.Orchestration;
using Queries;
using Commands;
using Core.EventSourcing.Aggregates;

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
