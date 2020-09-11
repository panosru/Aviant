namespace Aviant.DDD.Application.Orchestration
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Commands;
    using Domain.Aggregates;
    using Domain.Messages;
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
            PreUnitOfWork<TCommand, TCommandResponse>(TCommand command)
            where TCommand : class, IRequest<TCommandResponse>
        {
            var commandResponse = await _mediator.Send(command);

            // Fire pre/post notifications
            await _notificationDispatcher.FirePreCommitNotifications();

            List<string>? messages = null;

            if (_messages.HasMessages())
                messages = _messages.GetAll();

            return (commandResponse, messages);
        }

        protected dynamic? PostUnitOfWork<TCommandResponse>(TCommandResponse commandResponse)
        {
            // Fire post commit notifications
            Task.Run(() => _notificationDispatcher.FirePostCommitNotifications());

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

        public async Task<RequestResult> SendQuery<T>(IQuery<T> query)
        {
            var commandResponse = await _mediator.Send(query);

            return _messages.HasMessages()
                ? new RequestResult(_messages.GetAll())
                : new RequestResult(commandResponse);
        }
    }

    public class Orchestrator
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

        public async Task<RequestResult> SendCommand<T>(ICommand<T> command)
        {
            (var commandResponse, List<string>? messages) = await PreUnitOfWork<ICommand<T>, T>(command);

            if (!(messages is null))
                return new RequestResult(messages);

            var result = PostUnitOfWork(commandResponse);

            return new RequestResult(result);
        }

        #endregion
    }

    public class Orchestrator<TDbContext>
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

        public async Task<RequestResult> SendCommand<T>(ICommand<T> command)
        {
            (var commandResponse, List<string>? messages) = await PreUnitOfWork<ICommand<T>, T>(command);

            if (!(messages is null))
                return new RequestResult(messages);

            try
            {
                var affectedRows = await _unitOfWork.Commit()
                   .ConfigureAwait(false);

                var result = PostUnitOfWork(commandResponse);

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

    public class Orchestrator<TAggregate, TAggregateId>
        : OrchestratorBase,
          IOrchestrator<TAggregate, TAggregateId>
        where TAggregate : class, IAggregate<TAggregateId>
        where TAggregateId : class, IAggregateId
    {
        private readonly IUnitOfWork<TAggregate, TAggregateId> _unitOfWork;

        public Orchestrator(
            IUnitOfWork<TAggregate, TAggregateId> unitOfWork,
            IMessages                                 messages,
            INotificationDispatcher                   notificationDispatcher,
            IMediator                                 mediator)
            : base(messages, notificationDispatcher, mediator) => _unitOfWork = unitOfWork;

        #region IOrchestrator<TAggregate,TAggregateId> Members

        public async Task<RequestResult> SendCommand(ICommand<TAggregate, TAggregateId> command)
        {
            (var commandResponse, List<string>? messages) =
                await PreUnitOfWork<ICommand<TAggregate, TAggregateId>, TAggregate>(command);

            if (!(messages is null))
                return new RequestResult(messages);

            try
            {
                await _unitOfWork.Commit(commandResponse);
            }
            catch (Exception exception)
            {
                return new RequestResult(
                    new List<string>
                    {
                        exception.Message
                    });
            }
            
            var result = PostUnitOfWork(commandResponse);

            return new RequestResult(result);
        }

        #endregion
    }
}