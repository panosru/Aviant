namespace Aviant.DDD.Application.Orchestration
{
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using Core.Aggregates;
    using Persistance;
    using Queries;

    public interface IOrchestrator
    {
        public Task<OrchestratorResponse> SendCommandAsync<T>(
            ICommand<T>       command,
            CancellationToken cancellationToken = default);

        public Task<OrchestratorResponse> SendQueryAsync<T>(
            IQuery<T>         query,
            CancellationToken cancellationToken = default);
    }

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
}
