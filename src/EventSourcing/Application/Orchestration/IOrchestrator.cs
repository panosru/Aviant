using Aviant.Application.EventSourcing.Commands;
using Aviant.Application.Orchestration;
using Aviant.Application.Queries;
using Aviant.Core.EventSourcing.Aggregates;

namespace Aviant.Application.EventSourcing.Orchestration;

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
