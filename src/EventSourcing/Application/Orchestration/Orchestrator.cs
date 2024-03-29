using Aviant.Application.ApplicationEvents;
using Aviant.Application.EventSourcing.Commands;
using Aviant.Application.EventSourcing.Persistence;
using Aviant.Application.Orchestration;
using Aviant.Core.EventSourcing.Aggregates;
using Aviant.Core.Messages;
using MediatR;

namespace Aviant.Application.EventSourcing.Orchestration;

public sealed class Orchestrator<TAggregate, TAggregateId>
    : OrchestratorBase,
      IOrchestrator<TAggregate, TAggregateId>
    where TAggregate : class, IAggregate<TAggregateId>
    where TAggregateId : class, IAggregateId
{
    private readonly IUnitOfWork<TAggregate, TAggregateId> _unitOfWork;

    public Orchestrator(
        IUnitOfWork<TAggregate, TAggregateId> unitOfWork,
        IMessages                             messages,
        IApplicationEventDispatcher           applicationEventDispatcher,
        IMediator                             mediator)
        : base(messages, applicationEventDispatcher, mediator) => _unitOfWork = unitOfWork;

    #region IOrchestrator<TAggregate,TAggregateId> Members

    public async Task<OrchestratorResponse> SendCommandAsync(
        ICommand<TAggregate, TAggregateId> command,
        CancellationToken                  cancellationToken = default)
    {
        (var commandResponse, List<string>? messages) =
            await PreUnitOfWorkAsync<ICommand<TAggregate, TAggregateId>, TAggregate>(command, cancellationToken)
               .ConfigureAwait(false);

        if (messages is not null)
            return new OrchestratorResponse(messages);

        try
        {
            await _unitOfWork.CommitAsync(commandResponse, cancellationToken)
               .ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            return new OrchestratorResponse(
                new List<string>
                {
                    exception.Message
                });
        }

        var result = await PostUnitOfWorkAsync(commandResponse, cancellationToken)
           .ConfigureAwait(false);

        return new OrchestratorResponse(result);
    }

    #endregion
}
