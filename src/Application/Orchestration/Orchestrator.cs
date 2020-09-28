namespace Aviant.DDD.Application.Orchestration
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Commands;
    using Core.Aggregates;
    using Core.Messages;
    using MediatR;
    using Notifications;
    using Persistance;
    using Queries;

    public abstract class OrchestratorBase
    {
        private readonly IMediator _mediator;

        private readonly IMessages _messages;

        private readonly INotificationDispatcher _notificationDispatcher;

        protected OrchestratorBase(
            IMessages               messages,
            INotificationDispatcher notificationDispatcher,
            IMediator               mediator)
        {
            _messages               = messages;
            _notificationDispatcher = notificationDispatcher;
            _mediator               = mediator;
        }

        protected async Task<(TCommandResponse commandResponse, List<string>? _messages)>
            PreUnitOfWork<TCommand, TCommandResponse>(
                TCommand          command,
                CancellationToken cancellationToken = default)
            where TCommand : class, IRequest<TCommandResponse>
        {
            var commandResponse = await _mediator.Send(command, cancellationToken)
               .ConfigureAwait(false);

            // Fire pre/post notifications
            await _notificationDispatcher.FirePreCommitNotificationsAsync(cancellationToken)
               .ConfigureAwait(false);

            List<string>? messages = null;

            if (_messages.HasMessages())
                messages = _messages.GetAll();

            return (commandResponse, messages);
        }

        protected async Task<dynamic?> PostUnitOfWork<TCommandResponse>(
            TCommandResponse  commandResponse,
            CancellationToken cancellationToken)
        {
            // Fire post commit notifications
            await _notificationDispatcher.FirePostCommitNotificationsAsync(cancellationToken)
               .ConfigureAwait(false);

            var isLazy = false;

            try
            {
                isLazy = typeof(Lazy<>) == commandResponse?.GetType().GetGenericTypeDefinition();
            }
            catch (Exception)
            {
                // ignore
            }

            return isLazy
                ? commandResponse?.GetType().GetProperty("Value")?.GetValue(commandResponse, null)
                : commandResponse;
        }

        public async Task<RequestResult> SendQueryAsync<T>(
            IQuery<T>         query,
            CancellationToken cancellationToken = default)
        {
            var commandResponse = await _mediator.Send(query, cancellationToken)
               .ConfigureAwait(false);

            return _messages.HasMessages()
                ? new RequestResult(_messages.GetAll())
                : new RequestResult(commandResponse);
        }
    }

    public sealed class Orchestrator
        : OrchestratorBase,
          IOrchestrator
    {
        public Orchestrator(
            IMessages               messages,
            INotificationDispatcher notificationDispatcher,
            IMediator               mediator)
            : base(messages, notificationDispatcher, mediator)
        { }

        #region IOrchestrator Members

        public async Task<RequestResult> SendCommandAsync<T>(
            ICommand<T>       command,
            CancellationToken cancellationToken = default)
        {
            (var commandResponse, List<string>? messages) = await PreUnitOfWork<ICommand<T>, T>(
                    command,
                    cancellationToken)
               .ConfigureAwait(false);

            if (!(messages is null))
                return new RequestResult(messages);

            var result = await PostUnitOfWork(commandResponse, cancellationToken)
               .ConfigureAwait(false);

            return new RequestResult(result);
        }

        #endregion
    }

    public sealed class Orchestrator<TDbContext>
        : OrchestratorBase,
          IOrchestrator<TDbContext>
        where TDbContext : IDbContextWrite
    {
        private readonly IUnitOfWork<TDbContext> _unitOfWork;

        public Orchestrator(
            IUnitOfWork<TDbContext> unitOfWork,
            IMessages               messages,
            INotificationDispatcher notificationDispatcher,
            IMediator               mediator)
            : base(messages, notificationDispatcher, mediator) => _unitOfWork = unitOfWork;

        #region IOrchestrator<TDbContext> Members

        public async Task<RequestResult> SendCommandAsync<T>(
            ICommand<T>       command,
            CancellationToken cancellationToken = default)
        {
            (var commandResponse, List<string>? messages) = await PreUnitOfWork<ICommand<T>, T>(
                    command,
                    cancellationToken)
               .ConfigureAwait(false);

            if (!(messages is null))
                return new RequestResult(messages);

            try
            {
                var affectedRows = await _unitOfWork.CommitAsync(cancellationToken)
                   .ConfigureAwait(false);

                var result = await PostUnitOfWork(commandResponse, cancellationToken)
                   .ConfigureAwait(false);

                return new RequestResult(result, affectedRows);
            }
            catch (Exception exception)
            {
                return new RequestResult(
                    new List<string>
                    {
                        exception.Message
                    });
            }
        }

        #endregion
    }

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
            INotificationDispatcher               notificationDispatcher,
            IMediator                             mediator)
            : base(messages, notificationDispatcher, mediator) => _unitOfWork = unitOfWork;

        #region IOrchestrator<TAggregate,TAggregateId> Members

        public async Task<RequestResult> SendCommandAsync(
            ICommand<TAggregate, TAggregateId> command,
            CancellationToken                  cancellationToken = default)
        {
            (var commandResponse, List<string>? messages) =
                await PreUnitOfWork<ICommand<TAggregate, TAggregateId>, TAggregate>(command, cancellationToken)
                   .ConfigureAwait(false);

            if (!(messages is null))
                return new RequestResult(messages);

            try
            {
                await _unitOfWork.CommitAsync(commandResponse, cancellationToken)
                   .ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                return new RequestResult(
                    new List<string>
                    {
                        exception.Message
                    });
            }

            var result = await PostUnitOfWork(commandResponse, cancellationToken)
               .ConfigureAwait(false);

            return new RequestResult(result);
        }

        #endregion
    }
}