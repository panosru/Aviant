namespace Aviant.Application.Persistence.Orchestration;

using Application.Orchestration;
using ApplicationEvents;
using Commands;
using Core.Messages;
using MediatR;
using Persistence;

public sealed class Orchestrator<TDbContext>
    : OrchestratorBase,
      IOrchestrator<TDbContext>
    where TDbContext : IDbContextWrite
{
    private readonly IUnitOfWork<TDbContext> _unitOfWork;

    public Orchestrator(
        IUnitOfWork<TDbContext>     unitOfWork,
        IMessages                   messages,
        IApplicationEventDispatcher applicationEventDispatcher,
        IMediator                   mediator)
        : base(messages, applicationEventDispatcher, mediator) => _unitOfWork = unitOfWork;

    #region IOrchestrator<TDbContext> Members

    public async Task<OrchestratorResponse> SendCommandAsync<T>(
        ICommand<T>       command,
        CancellationToken cancellationToken = default)
    {
        (var commandResponse, List<string>? messages) = await PreUnitOfWorkAsync<ICommand<T>, T>(
                command,
                cancellationToken)
           .ConfigureAwait(false);

        if (messages is not null)
            return new OrchestratorResponse(messages);

        try
        {
            var affectedRows = await _unitOfWork.CommitAsync(cancellationToken)
               .ConfigureAwait(false);

            var result = await PostUnitOfWorkAsync(commandResponse, cancellationToken)
               .ConfigureAwait(false);

            return new OrchestratorResponse(result, affectedRows);
        }
        catch (Exception exception)
        {
            return new OrchestratorResponse(
                new List<string>
                {
                    exception.Message
                });
        }
    }

    #endregion
}
